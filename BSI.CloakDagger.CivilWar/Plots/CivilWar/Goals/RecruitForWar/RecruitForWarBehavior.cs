using BSI.CloakDager.CivilWar.Settings;
using BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar
{
    public class RecruitForWarBehavior : Behavior
    {
        private static readonly CivilWarSettings settings = CivilWarSettings.Instance;

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(OnDailyTick));
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        private void OnDailyTick()
        {
            if (!Goal.IsActive)
            {
                return;
            }

            var plot = Goal.Plot;

            foreach (var clan in plot.Target.ConvertToKingdom()?.Clans)
            {
                CheckForEnter(clan, plot);
            }

            foreach (var plotter in plot.Members)
            {
                CheckForNewLeader(plotter, plot);
                CheckForLeave(plotter, plot);
            }
        }

        public override bool CanEnd()
        {
            var plot = Goal.Plot;

            var plotLeaderHero = plot.Leader.ConvertToHero();
            var plotTargetHero = plot.Target.ConvertToHero();
            var plotLeaderKingdom = plot.Leader.ConvertToKingdom();
            var plotTargetKingdom = plot.Target.ConvertToKingdom();

            double warPersonality
                = plotLeaderHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + plotLeaderHero.GetCharacterTraitLevel(CharacterTrait.Mercy)
                + plotTargetHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + plotTargetHero.GetCharacterTraitLevel(CharacterTrait.Mercy);
            double valorFactor
                = Math.Pow(GetStrength(plot) / (plotTargetHero.Clan.Kingdom.TotalStrength - GetStrength(plot)),
                plotLeaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) == 0 ? 1 : 2 * plotTargetHero.GetCharacterTraitLevel(CharacterTrait.Valor));
            double warPartyFactor
                = Math.Pow(GetWarPartiesCount(plot) / (plotTargetHero.Clan.Kingdom.WarParties.Count() - GetWarPartiesCount(plot)),
                1 + plotLeaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating));
            double warchance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;

            Debug.AddEntry($"War Chance in {plotTargetHero.Clan.Kingdom.Name} || {warchance}");

            return new Random().Next(100) < warchance
                && plotLeaderHero.MapFaction != plotTargetHero.MapFaction
                && FactionManager.GetEnemyKingdoms(plotLeaderKingdom).Contains(plotTargetKingdom);
        }

        public override bool DoEnd()
        {
            var plot = Goal.Plot;

            var name = new TextObject(plot.Target.ConvertToKingdom().InformalName.ToString() + " Seperatists");
            var informalname = new TextObject(plot.Leader.ConvertToHero().Clan.InformalName.ToString());

            var oldKingdom = plot.Target.ConvertToKingdom();
            var rebel = CloakDagger.Managers.KingdomManager.CreateKingdom(plot.Leader.ConvertToHero(), name, informalname, plot.Leader.ConvertToHero().Clan.Banner, true);

            foreach (var member in plot.Members)
            {
                if (member.ConvertToHero().Clan.Kingdom != rebel)
                {
                    ChangeKingdomAction.ApplyByJoinToKingdom(member.ConvertToHero().Clan, rebel, true);
                }

            }

            DeclareWarAction.Apply(rebel, oldKingdom);

            foreach (var hero in oldKingdom.Lords)
            {
                var warRelationChange = settings.WarRelationshipChange;
                var allyRelationChange = settings.AllyRelationshipChange / 2;

                if (hero.IsFactionLeader())
                {
                    warRelationChange *= settings.LeaderRelationshipChangeFactor;
                    allyRelationChange *= settings.LeaderRelationshipChangeFactor;
                }

                foreach (var ally in oldKingdom.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int)allyRelationChange);
                }

                foreach (var enemy in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, enemy, (int)warRelationChange);
                }

                foreach (var ally in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int)allyRelationChange);
                }
            }

            InformationManager.AddNotice(new WarMapNotification(rebel, oldKingdom, new TextObject($"Civil War breaks out in {oldKingdom.Name}")));

            Debug.AddEntry("Successful Revolt created: " + rebel.Name.ToString());

            plot.CurrentGoal.SetNextGoal(typeof(WarForIndependenceGoal));
            return true;
        }

        private bool CheckForEnter(Clan clan, Plot plot)
        {
            if (!plot.Members.Contains(clan) && CanPlot(clan, plot) && WantPlot(clan, plot))
            {
                plot.Members.Add(clan);
                return true;
            }

            return false;
        }

        private bool CheckForLeave(MBObjectBase member, Plot plot)
        {
            var memberLeader = member.ConvertToHero();
            var plotLeader = plot.Leader.ConvertToHero();
            var plotTarget = plot.Target.ConvertToHero();

            if (memberLeader.IsClanLeader()
                && memberLeader.GetRelation(plotTarget) > settings.PositiveRelationThreshold
                && memberLeader.GetRelation(plotTarget) > memberLeader.GetRelation(plotLeader))
            {
                return plot.Members.Remove(member);
            }

            return false;
        }

        private bool CheckForNewLeader(MBObjectBase member, Plot plot)
        {
            var memberLeader = member.ConvertToHero();
            var plotLeader = plot.Leader.ConvertToHero();

            if (plot.Leader is null)
            {
                plot.Leader = member;
                return true;
            }

            if (memberLeader.Clan.Tier >= plotLeader.Clan.Tier && memberLeader.GetPlottingFriends(plot).Count > plotLeader.GetPlottingFriends(plot).Count)
            {
                plot.Leader = member;
                return true;
            }

            return false;
        }

        private bool CanPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            return hero.Clan.Kingdom.Leader != hero
                && hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold
                && !hero.Clan.IsMinorFaction;
        }

        private bool WantPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            var plottingFriends = hero.GetPlottingFriends(plot);

            var honorScore = -(hero.GetCharacterTraitLevel(CharacterTrait.Honor) + hero.Clan.Kingdom.Leader.GetCharacterTraitLevel(CharacterTrait.Honor));

            return settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends.Count) > new Random().Next(100);
        }

        private int GetWarPartiesCount(Plot plot)
        {
            var parties = 0;
            var clansDone = new List<Clan>();

            foreach (var hero in plot.Members.ConvertToHeroes())
            {
                if (clansDone.Contains(hero.Clan))
                {
                    break;
                }

                foreach (var party in hero.Clan.WarParties)
                {
                    parties += 1;
                }
            }

            return parties;
        }

        private double GetStrength(Plot plot)
        {
            var totalStrength = 0f;
            var clansDone = new List<Clan>();

            foreach (var hero in plot.Members.ConvertToHeroes())
            {
                if (clansDone.Contains(hero.Clan))
                {
                    break;
                }

                foreach (var party in hero.Clan.WarParties)
                {
                    totalStrength += party.GetTotalStrengthWithFollowers();
                }
            }

            return totalStrength;
        }
    }
}
using BSI.CivilWar.Goals.WarForIndependence;
using BSI.Core;
using BSI.Core.Enumerations;
using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Goals
{
    public class RecruitForWarBehavior : Behavior
    {
        private static readonly MySettings settings = MySettings.Instance;

        public override bool CanEnd(Plot plot)
        {
            var plotLeader = plot.Leader.ConvertToHero();
            var plotTarget = plot.Target.ConvertToHero();

            double warPersonality
                = plotLeader.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + plotLeader.GetCharacterTraitLevel(CharacterTrait.Mercy)
                + plotTarget.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + plotTarget.GetCharacterTraitLevel(CharacterTrait.Mercy);
            double valorFactor
                = Math.Pow (this.GetStrength(plot) / (plotTarget.Clan.Kingdom.TotalStrength - this.GetStrength(plot)),
                plotLeader.GetCharacterTraitLevel(CharacterTrait.Valor) == 0 ? 1 : 2 * plotTarget.GetCharacterTraitLevel(CharacterTrait.Valor));
            double warPartyFactor
                = Math.Pow(this.WarParties(plot) / (plotTarget.Clan.Kingdom.WarParties.Count() - this.WarParties(plot)),
                1 + plotLeader.GetCharacterTraitLevel(CharacterTrait.Calculating));
            double warchance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;

            Debug.AddEntry($"War Chance in {plotTarget.Clan.Kingdom.Name} || {warchance}");

            return new Random().Next(100) < warchance;
        }

        public override bool DoEnd(Plot plot)
        {
            var name = new TextObject(plot.Target.ConvertToKingdom().InformalName.ToString() + " Seperatists");
            var informalname = new TextObject(plot.Leader.ConvertToHero().Clan.InformalName.ToString());

            var oldKingdom = plot.Target.ConvertToHero().Clan.Kingdom;
            var rebel = Core.Managers.KingdomManager.CreateKingdom(plot.Leader.ConvertToHero(), name, informalname, plot.Leader.ConvertToHero().Clan.Banner, true);

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
                float warRelationChange = settings.WarRelationshipChange;
                float allyRelationChange = settings.AllyRelationshipChange / 2;

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

            InformationManager.AddNotice(new WarMapNotification(rebel, oldKingdom, new TextObject("Civil War breaks out in " + oldKingdom.Name + "!!!")));

            Debug.AddEntry("Successful Revolt created: " + rebel.Name.ToString());

            plot.CurrentGoal = plot.CurrentGoal.NextGoals.FirstOrDefault(goal => goal.GetType() == typeof(WarForIndependenceGoal));

            return true;
        }

        public override void OnDailyTick(Plot plot)
        {
            foreach (var clan in plot.Target.ConvertToHero().Clan.Kingdom.Clans)
            {
                this.DoPlot(clan, plot);
            }

            foreach (var plotter in plot.Members)
            {

                this.IsNewLeader(plotter, plot);
                this.LeaveCondition(plotter, plot);
            }

            if (this.CanEnd(plot) || plot.CurrentGoal.EndCondition)
            {
                this.DoEnd(plot);
            }
        }

        private bool CanPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            return hero.Clan.Kingdom.Leader != hero
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !plot.Members.Contains(hero)
                && !hero.Clan.IsMinorFaction;
        }

        private bool WantPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            var plottingFriends = hero.GetPlottingFriends(plot);

            var honorScore = -(HeroExtension.GetCharacterTraitLevel(hero, CharacterTrait.Honor) + HeroExtension.GetCharacterTraitLevel(hero.Clan.Kingdom.Leader, CharacterTrait.Honor));

            return settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends.Count) > new Random().Next(100);
        }

        private bool DoPlot(Clan clan, Plot plot)
        {
            if (this.CanPlot(clan, plot) && this.WantPlot(clan, plot))
            {
                plot.Members.Add(clan);
                return true;
            }

            return false;
        }

        public bool IsNewLeader(MBObjectBase member, Plot plot)
        {
            var memberLeader = member.ConvertToHero();
            var plotLeader = plot.Leader.ConvertToHero();

            if (plot.Leader is null)
            {
                plot.Leader = member;
                return true;
            }
            else if (memberLeader.Clan.Tier >= plotLeader.Clan.Tier && HeroExtension.GetPlottingFriends(memberLeader, plot).Count > HeroExtension.GetPlottingFriends(plotLeader, plot).Count)
            {
                plot.Leader = member;
                return true;
            }

            return false;
        }

        public bool LeaveCondition(MBObjectBase member, Plot plot)
        {
            var memberLeader = member.ConvertToHero();
            var plotLeader = plot.Leader.ConvertToHero();

            if (HeroExtension.IsClanLeader(memberLeader)
                && memberLeader.GetRelation(plot.Target.ConvertToHero()) > settings.PositiveRelationThreshold
                && memberLeader.GetRelation(plot.Target.ConvertToHero()) > memberLeader.GetRelation(plotLeader))
            {
                return plot.Members.Remove(member);
            }

            return false;
        }

        private int WarParties(Plot plot)
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

            foreach (Hero hero in plot.Members.ConvertToHeroes())
            {
                if(clansDone.Contains(hero.Clan))
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
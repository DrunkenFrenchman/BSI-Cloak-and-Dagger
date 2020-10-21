using BSI.CloakDagger.CivilWar.Settings;
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
    internal class RecruitForWarBehavior : Behavior
    {
        private static readonly CivilWarSettings settings = CivilWarSettings.Instance;

        public override bool CanEnd()
        {
            var plot = Goal.Plot;

            var leaderHero = plot.Leader.ConvertToHero();
            var targetHero = plot.Target.ConvertToHero();
            var strength = GetStrength(plot);
            var warPartiesCount = GetWarPartiesCount(plot);

            /*var warPersonality
                = leaderHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + leaderHero.GetCharacterTraitLevel(CharacterTrait.Mercy)
                + targetHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + targetHero.GetCharacterTraitLevel(CharacterTrait.Mercy);
            var valorFactor
                = Math.Pow(strength / (targetHero.Clan.Kingdom.TotalStrength - strength),
                leaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) == 0 ? 1 : 2 * targetHero.GetCharacterTraitLevel(CharacterTrait.Valor));
            var warPartyFactor
                = Math.Pow((double)warPartiesCount / (double)(targetHero.Clan.Kingdom.WarParties.Count() - warPartiesCount),
                1 + leaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating));
            var warChance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;*/

            var personalityFactor
                = Math.Pow(settings.WarPersonalityMult,
                -(leaderHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + leaderHero.GetCharacterTraitLevel(CharacterTrait.Mercy)
                + targetHero.GetCharacterTraitLevel(CharacterTrait.Generosity)
                + targetHero.GetCharacterTraitLevel(CharacterTrait.Mercy)));
            var strengthFactor = strength / (targetHero.Clan.Kingdom.TotalStrength - strength);
            var valorFactor
                = 1 + (leaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) <= 0 ? 1 : leaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) * 2);
            var partiesFactor
                = Math.Pow((double)warPartiesCount / (double)(targetHero.Clan.Kingdom.WarParties.Count() - warPartiesCount),
                1 + (leaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating) <= 0 ? 1 : leaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating)));

            var warChance = settings.WarBaseChance * personalityFactor * (strengthFactor * valorFactor) * partiesFactor;

            Debug.AddEntry($"War Chance in {targetHero.Clan.Kingdom.Name}: {warChance}");

            return new Random().Next(100) < warChance;
        }

        public override bool DoEnd()
        {
            var plot = Goal.Plot;

            var name = new TextObject(plot.Target.ConvertToKingdom().InformalName.ToString() + " Seperatists");
            var informalname = new TextObject(plot.Leader.ConvertToHero().Clan.InformalName.ToString());

            var oldKingdom = plot.Target.ConvertToKingdom();
            var rebel = Managers.KingdomManager.CreateKingdom(plot.Leader.ConvertToHero(), name, informalname, plot.Leader.ConvertToHero().Clan.Banner, true);

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

            Goal.SetNextGoal(typeof(WarForIndependenceGoal));
            return true;
        }

        public override void DailyTick()
        {
            if (Goal == null || Goal != Goal?.Plot?.CurrentGoal)
            {
                return;
            }

            var plot = Goal.Plot;

            var membersToAdd = new List<MBObjectBase>();
            foreach (var clan in plot.Target.ConvertToKingdom()?.Clans)
            {
                if (CheckForEnter(clan, plot))
                {
                    membersToAdd.Add(clan.Leader);
                }
            }

            plot.Members.AddRange(membersToAdd);

            var membersToRemove = new List<MBObjectBase>();
            foreach (var plotter in plot.Members)
            {
                CheckForNewLeader(plotter, plot);
                if (CheckForLeave(plotter, plot))
                {
                    membersToRemove.Add(plotter);
                }
            }

            plot.Members = plot.Members.Except(membersToRemove).ToList();
        }

        private bool CheckForEnter(Clan clan, Plot plot)
        {
            if (!plot.Members.Contains(clan.Leader) && CanPlot(clan, plot) && WantPlot(clan, plot))
            {
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
                return true;
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
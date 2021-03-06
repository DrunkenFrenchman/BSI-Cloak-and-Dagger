﻿using System;
using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.CivilWar.Settings;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Models;
using BSI.CloakDagger.Models.Comparer;
using BSI.CloakDagger.Models.PlotMod;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar
{
    public class RecruitForWarBehavior : Behavior
    {
        private static readonly CivilWarSettings Settings = CivilWarSettings.Instance;

        public override bool CanEnd()
        {
            var plot = Goal.Plot;

            var leaderHero = plot.Leader.ToHero();
            var targetHero = plot.Target.ToHero();
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

            var personalityFactor = Math.Pow(Settings.WarPersonalityMult, -(leaderHero.GetCharacterTraitLevel(CharacterTrait.Generosity) + leaderHero.GetCharacterTraitLevel(CharacterTrait.Mercy) + targetHero.GetCharacterTraitLevel(CharacterTrait.Generosity) + targetHero.GetCharacterTraitLevel(CharacterTrait.Mercy)));
            var strengthFactor = strength / (targetHero.Clan.Kingdom.TotalStrength - strength);
            var valorFactor = 1 + (leaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) <= 0 ? 1 : leaderHero.GetCharacterTraitLevel(CharacterTrait.Valor) * 2);
            var partiesFactor = Math.Pow(warPartiesCount / (double) (targetHero.Clan.Kingdom.WarParties.Count() - warPartiesCount), 1 + (leaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating) <= 0 ? 1 : leaderHero.GetCharacterTraitLevel(CharacterTrait.Calculating)));

            var warChance = Settings.WarBaseChance * personalityFactor * (strengthFactor * valorFactor) * partiesFactor;

            LogHelper.Log($"War Chance in {targetHero.Clan.Kingdom.Name}: {warChance}");

            return new Random().Next(100) < warChance;
        }

        public override void DoEnd()
        {
            var plot = Goal.Plot;

            var name = new TextObject(plot.Target.ToKingdom().InformalName + " Seperatists");
            var informalName = new TextObject(plot.Leader.ToHero().Clan.InformalName.ToString());

            var oldKingdom = plot.Target.ToKingdom();
            var rebel = KingdomHelper.CreateKingdom(plot.Leader.ToHero(), name, informalName, plot.Leader.ToHero().Clan.Banner, true);

            foreach (var member in plot.Members.Where(member => member.ToHero().Clan.Kingdom != rebel))
            {
                ChangeKingdomAction.ApplyByJoinToKingdom(member.ToHero().Clan, rebel);
            }

            DeclareWarAction.Apply(rebel, oldKingdom);

            foreach (var hero in oldKingdom.Lords)
            {
                var warRelationChange = Settings.WarRelationshipChange;
                var allyRelationChange = Settings.AllyRelationshipChange / 2;

                if (hero.IsFactionLeader())
                {
                    warRelationChange *= Settings.LeaderRelationshipChangeFactor;
                    allyRelationChange *= Settings.LeaderRelationshipChangeFactor;
                }

                foreach (var ally in oldKingdom.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int) allyRelationChange);
                }

                foreach (var enemy in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, enemy, (int) warRelationChange);
                }

                foreach (var ally in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int) allyRelationChange);
                }
            }

            InformationManager.AddNotice(new WarMapNotification(rebel, oldKingdom, new TextObject($"Civil War breaks out in {oldKingdom.Name}")));

            LogHelper.Log("Successful Revolt created: " + rebel.Name);
        }

        internal void DailyTick()
        {
            var plot = Goal.Plot;

            plot.Members.AddRange(plot.Target.ToKingdom().Clans.Where(c => CheckForEnter(c, plot)).Select(clan => clan.ToGameObject()));

            var membersToRemove = new List<GameObject>();
            foreach (var plotter in plot.Members)
            {
                var clanLeader = plotter.ToClan().Leader;

                CheckForNewLeader(clanLeader, plot);
                if (CheckForLeave(clanLeader, plot))
                {
                    membersToRemove.Add(plotter);
                }
            }

            plot.Members = plot.Members.Except(membersToRemove, new GameObjectComparer()).ToList();
        }

        private static bool CheckForEnter(Clan clan, Plot plot)
        {
            return !plot.Members.ToClans().Contains(clan) && CanPlot(clan, plot) && WantPlot(clan, plot);
        }

        private static bool CheckForLeave(Hero member, Plot plot)
        {
            var plotLeader = plot.Leader.ToHero();
            var plotTarget = plot.Target.ToHero();

            return member.IsClanLeader() && member.GetRelation(plotTarget) > Settings.PositiveRelationThreshold && member.GetRelation(plotTarget) > member.GetRelation(plotLeader);
        }

        private static void CheckForNewLeader(Hero member, Plot plot)
        {
            var plotLeader = plot.Leader.ToHero();

            if (!plot.Members.Contains(plot.Leader))
            {
                plot.Leader.StringId = member.StringId;
                return;
            }

            if (member.Clan.Tier < plotLeader.Clan.Tier || member.GetPlottingFriends(plot).Count <= plotLeader.GetPlottingFriends(plot).Count)
            {
                return;
            }

            plot.Leader.StringId = member.StringId;
        }

        private static bool CanPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            var kingdomLeader = plot.Target.ToHero();
            return kingdomLeader != hero && hero.GetRelation(kingdomLeader) < Settings.NegativeRelationThreshold && !clan.IsMinorFaction;
        }

        private static bool WantPlot(Clan clan, Plot plot)
        {
            var hero = clan.Leader;
            var plottingFriends = hero.GetPlottingFriends(plot);

            var honorScore = -(hero.GetCharacterTraitLevel(CharacterTrait.Honor) + clan.Kingdom.Leader.GetCharacterTraitLevel(CharacterTrait.Honor));

            return Settings.BasePlotChance * Math.Pow(Settings.PlotPersonalityMult, honorScore) * Math.Pow(Settings.PlotFriendMult, plottingFriends.Count) > new Random().Next(100);
        }

        private static int GetWarPartiesCount(Plot plot)
        {
            var parties = 0;
            var clansDone = new List<Clan>();

            foreach (var hero in from hero in plot.Members.ToHeroes().TakeWhile(hero => !clansDone.Contains(hero.Clan)) from party in hero.Clan.WarParties select hero)
            {
                parties += 1;
                clansDone.Add(hero.Clan);
            }

            return parties;
        }

        private static double GetStrength(Plot plot)
        {
            var totalStrength = 0f;
            var clansDone = new List<Clan>();

            foreach (var hero in plot.Members.ToHeroes())
            {
                if (clansDone.Contains(hero.Clan))
                {
                    break;
                }

                foreach (var party in hero.Clan.WarParties)
                {
                    totalStrength += party.GetTotalStrengthWithFollowers();
                    clansDone.Add(hero.Clan);
                }
            }

            return totalStrength;
        }
    }
}
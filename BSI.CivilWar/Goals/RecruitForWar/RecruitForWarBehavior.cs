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
            int tick = new Random().Next(100);
            double warPersonality
                = HeroExtension.GetCharacterTraitLevel(HeroExtension.ConvertToHero(plot.Leader), CharacterTrait.Generosity)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.ConvertToHero(plot.Leader), CharacterTrait.Mercy)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Generosity)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Mercy);
            double valorFactor
                = Math.Pow((this.GetStrength(plot) / (HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom.TotalStrength - this.GetStrength(plot))),
                HeroExtension.GetCharacterTraitLevel(HeroExtension.ConvertToHero(plot.Leader), CharacterTrait.Valor) == 0 ? 1 : 2 * HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Valor));
            double warPartyFactor
                = Math.Pow((this.WarParties(plot) / (HeroExtension.ConvertToHero(plot.Parent).Clan.Kingdom.WarParties.Count() - this.WarParties(plot))),
                1 + HeroExtension.GetCharacterTraitLevel(HeroExtension.ConvertToHero(plot.Leader), CharacterTrait.Calculating));
            double warchance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;

            Debug.AddEntry("War Chance in " + HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom.Name.ToString() + " || " + warchance.ToString());

            return tick < warchance;
        }

        public override bool DoEnd(Plot plot)
        {
            TextObject name = new TextObject(HeroExtension.ConvertToKingdom(plot.Target).InformalName.ToString() + " Seperatists");
            TextObject informalname = new TextObject(HeroExtension.ConvertToHero(plot.Leader).Clan.InformalName.ToString());

            Kingdom rebel = Core.Managers.KingdomManager.CreateKingdom(HeroExtension.ConvertToHero(plot.Leader), name, informalname, HeroExtension.ConvertToHero(plot.Leader).Clan.Banner, true);

            foreach (MBObjectBase member in plot.Members)
            {
                if (!HeroExtension.ConvertToHero(member).Clan.Kingdom.Equals(rebel))
                {
                    ChangeKingdomAction.ApplyByJoinToKingdom(HeroExtension.ConvertToHero(member).Clan, rebel, true);
                }
                
            }
            Kingdom oldKingdom = HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom;

            DeclareWarAction.Apply(rebel, oldKingdom);

            foreach (Hero hero in oldKingdom.Lords)
            {
                float warRelationChange = settings.WarRelationshipChange;
                float allyRelationChange = settings.AllyRelationshipChange / 2;
                if (HeroExtension.IsFactionLeader(hero)) { warRelationChange *= settings.LeaderRelationshipChangeFactor; allyRelationChange *= settings.LeaderRelationshipChangeFactor; }
                foreach (Hero ally in oldKingdom.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int)allyRelationChange);
                }

                foreach (Hero enemy in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, enemy, (int)warRelationChange);
                }

                foreach (Hero ally in rebel.Lords)
                {
                    ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero, ally, (int)allyRelationChange);
                }
            }

            InformationManager.AddNotice(new WarMapNotification(rebel, oldKingdom, new TextObject("Civil War breaks out in " + oldKingdom.Name + "!!!")));

            Debug.AddEntry("Successful Revolt created: " + rebel.Name.ToString());

            plot.CurrentGoal = plot.CurrentGoal.NextGoals.FirstOrDefault(goal => goal.GetType().Equals(typeof(WarForIndependenceGoal)));

            return true;
        }

        public override void OnDailyTick(Plot plot)
        {
            foreach (Clan clan in HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom.Clans)
            {
                DoPlot(clan, plot);
            }

            foreach (MBObjectBase plotter in plot.Members)
            {

                IsNewLeader(plotter, plot);
                LeaveCondition(plotter, plot);
            }

            if (CanEnd(plot) || plot.CurrentGoal.EndCondition)
            {
                DoEnd(plot);
            }
            return;
        }

        private bool CanPlot(Clan clan, Plot plot)
        {
            Hero hero = clan.Leader;
            return (!hero.Clan.Kingdom.Leader.Equals(hero)
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !plot.Members.Contains(hero)
                && !hero.Clan.IsMinorFaction);
        }

        private bool WantPlot(Clan clan, Plot plot)
        {
            Hero hero = clan.Leader;
            int tick = new Random().Next(100);
            int honorScore = -(HeroExtension.GetCharacterTraitLevel(hero, CharacterTrait.Honor) + HeroExtension.GetCharacterTraitLevel(hero.Clan.Kingdom.Leader, CharacterTrait.Honor));
            List<Hero> plottingFriends = HeroExtension.GetPlottingFriends(hero, plot);
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends.Count);
            return plottingChance > tick;
        }

        private bool DoPlot(Clan clan, Plot plot)
        {
            if (CanPlot(clan, plot) && WantPlot(clan, plot))
            {
                plot.Members.Add(clan);
                return true;
            }
            return false;
        }

        public bool IsNewLeader(MBObjectBase member, Plot plot)
        {
            Hero memberLeader = HeroExtension.ConvertToHero(member);
            Hero plotLeader = HeroExtension.ConvertToHero(plot.Leader);

            if (plot.Leader is null) { plot.Leader = member; return true; }
            else if (memberLeader.Clan.Tier >= plotLeader.Clan.Tier && HeroExtension.GetPlottingFriends(memberLeader, plot).Count > HeroExtension.GetPlottingFriends(plotLeader, plot).Count)
            {
                plot.Leader = member;
                return true;
            }
            return false;
        }

        public bool LeaveCondition(MBObjectBase member, Plot plot)
        {
            Hero memberLeader = HeroExtension.ConvertToHero(member);
            Hero plotLeader = HeroExtension.ConvertToHero(plot.Leader);
            if (HeroExtension.IsClanLeader(memberLeader) && (memberLeader.GetRelation(HeroExtension.ConvertToHero(plot.Target)) > settings.PositiveRelationThreshold
                && memberLeader.GetRelation(HeroExtension.ConvertToHero(plot.Target)) > memberLeader.GetRelation(plotLeader)))
            {
                return plot.Members.Remove(member);
            }
            return false;
        }

        private int WarParties(Plot plot)
        {
            int parties = 0;
            List<Clan> clansDone = new List<Clan>();
            foreach (Hero hero in HeroExtension.ConvertToHero(plot.Members))
            {
                if (clansDone.Contains(hero.Clan)) { break; }
                foreach (MobileParty party in hero.Clan.WarParties)
                {
                    parties += 1;
                }
            }
            return parties;
        }

        private double GetStrength(Plot plot)
        {
            double TotalStrength = 0;
            List<Clan> clansDone = new List<Clan>();
            foreach (Hero hero in HeroExtension.ConvertToHero(plot.Members))
            {
                if(clansDone.Contains(hero.Clan)) { break; }
                foreach (MobileParty party in hero.Clan.WarParties)
                {
                    TotalStrength += party.GetTotalStrengthWithFollowers();
                }
            }
            return TotalStrength;
        }
    }
}
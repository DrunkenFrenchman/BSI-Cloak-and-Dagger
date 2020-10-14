﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using BSI.Core.Objects;
using BSI.Core.Tools;
using Messages.FromLobbyServer.ToClient;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public sealed class RecruitforCivilWarB : Behavior
    {
        private static readonly BSI.Core.MySettings settings = BSI.Core.MySettings.Instance;

        private readonly Plot ThisPlot;

        public RecruitforCivilWarB()
        {

        }

        public override bool EndCondition()
        {
            int tick = new Random().Next(100);
            double warPersonality 
                = BSI_Hero.GetTraitLevel(ThisPlot.Leader, BSI_Hero.HeroTraits.Generosity)
                + BSI_Hero.GetTraitLevel(ThisPlot.Leader, BSI_Hero.HeroTraits.Mercy)
                + BSI_Hero.GetTraitLevel(ThisPlot.ParentFaction.Leader, BSI_Hero.HeroTraits.Generosity)
                + BSI_Hero.GetTraitLevel(ThisPlot.ParentFaction.Leader, BSI_Hero.HeroTraits.Mercy);
            double valorFactor
                = Math.Pow((ThisPlot.TotalStrength / (ThisPlot.ParentFaction.TotalStrength - ThisPlot.TotalStrength)),
                BSI_Hero.GetTraitLevel(ThisPlot.Leader, BSI_Hero.HeroTraits.Valor) == 0 ? 1 : 2 * BSI_Hero.GetTraitLevel(ThisPlot.Leader, BSI_Hero.HeroTraits.Valor));
            double warPartyFactor
                = Math.Pow((ThisPlot.WarParties / (ThisPlot.ParentFaction.WarParties.Count() - ThisPlot.WarParties)),
                1 + BSI_Hero.GetTraitLevel(ThisPlot.Leader, BSI_Hero.HeroTraits.Calc));

            return tick < settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;
        }

        public override bool EndResult()
        {
            Debug.PrintMessage("END OF PLOTTING REACHED\nEND OF PLOTTING REACHED\nEND OF PLOTTING REACHED\n");
            return true;
        }

        public override bool OnDailyTick()
        {
            foreach (Hero hero in ThisPlot.ParentFaction.Heroes)
            {
                DoPlot(hero);
            }

            foreach (Hero plotter in ThisPlot.ClanLeaders)
            {
                LeaveCondition(plotter);
                IsNewLeader(plotter);
            }

            if (EndCondition() && !ThisPlot.CurrentGoal.IsEndGoal)
            {
                ThisPlot.CurrentGoal = ThisPlot.CurrentGoal.NextGoal;
            }
            else { EndResult(); }
            return true;
        }

        internal override bool CanPlot(Hero hero)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero) 
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !ThisPlot.Members.Contains(hero));
        }

        public override bool WantPlot(Hero hero)
        {
            int tick = new Random().Next(100);
            int honorScore = -(BSI_Hero.GetTraitLevel(hero, BSI_Hero.HeroTraits.Honor) + BSI_Hero.GetTraitLevel(hero.Clan.Kingdom.Leader, BSI_Hero.HeroTraits.Honor));
            int plottingFriends = BSI_Hero.GetPlottingFriends(hero, ThisPlot).Count();
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends);
            return plottingChance > tick;
        }

        public override bool DoPlot(Hero hero)
        {
            if (BSI_Hero.IsClanLeader(hero) && CanPlot(hero) && WantPlot(hero)) 
            {
                if (ThisPlot != null) { return ThisPlot.AddMember(hero); }
                return false; 
            }
            return false;
        }

        public override bool IsNewLeader(Hero hero)
        {
            if (ThisPlot.Leader is null) { ThisPlot.Leader = hero; return true; }
            else if (hero.Clan.Tier >= this.ThisPlot.Leader.Clan.Tier && BSI_Hero.GetPlottingFriends(hero, ThisPlot).Count() > BSI_Hero.GetPlottingFriends(ThisPlot.Leader, ThisPlot).Count())
            {
                ThisPlot.Leader = hero;
                return true;
            }
            return false;
        }

        public override bool LeaveCondition(Hero hero)
        {
            if ((hero.GetRelation(ThisPlot.ParentFaction.Leader) > settings.PositiveRelationThreshold
                && hero.GetRelation(ThisPlot.ParentFaction.Leader) > hero.GetRelation(ThisPlot.Leader)))
            {
                return ThisPlot.RemoveMember(hero);
            }
            return false;
        }
    }
}

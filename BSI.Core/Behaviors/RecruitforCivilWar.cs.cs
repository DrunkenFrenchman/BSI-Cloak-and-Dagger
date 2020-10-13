﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using BSI.Core.Tools;
using BSI.Manager;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Behaviors
{
    public sealed class RecruitforCivilWar : BehaviorCore
    {
        private static readonly MySettings settings = new MySettings();

        private readonly Plot ThisPlot;
        public RecruitforCivilWar(Goal goal)
        {
            this.ThisPlot = goal.GetPlot;
        }

        public override bool EndCondition()
        {
            int tick = new Random().Next(100);

            throw new NotImplementedException();
        }

        public override bool OnDailyTick()
        {
            foreach (Hero hero in ThisPlot.OriginalFaction.Heroes)
            {
                JoinCondition(hero);
            }

            foreach (Hero plotter in ThisPlot.ClanLeaders)
            {
                LeaveCondition(plotter);
                IsNewLeader(plotter);
            }

            return true;
        }

        internal override bool CanPlot(Hero hero)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero) && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold));
        }

        internal bool StartedPlotting(Hero hero)
        {
            int tick = new Random().Next(100);
            int honorScore = -(BSI_Hero.GetTraitLevel(hero, BSI_Hero.HeroTraits.Honor) + BSI_Hero.GetTraitLevel(hero.Clan.Kingdom.Leader, BSI_Hero.HeroTraits.Honor));
            int plottingFriends = BSI_Hero.GetPlottingFriends(hero, ThisPlot).Count();
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends);
            return plottingChance > tick;
        }

        public override bool JoinCondition(Hero hero)
        {
            if (BSI_Hero.IsClanLeader(hero) && CanPlot(hero) && StartedPlotting(hero)) 
            {
                if (ThisPlot != null) { ThisPlot.AddMember(hero); return true; }
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
            return (hero.GetRelation(ThisPlot.OriginalFaction.Leader) > settings.PositiveRelationThreshold 
                && hero.GetRelation(ThisPlot.OriginalFaction.Leader) > hero.GetRelation(ThisPlot.Leader));
        }
    }
}
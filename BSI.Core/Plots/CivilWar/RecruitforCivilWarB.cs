using System;
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
                = HeroTools.GetTraitLevel(ThisPlot.Leader, HeroTools.HeroTraits.Generosity)
                + HeroTools.GetTraitLevel(ThisPlot.Leader, HeroTools.HeroTraits.Mercy)
                + HeroTools.GetTraitLevel(ThisPlot.ParentFaction.Leader, HeroTools.HeroTraits.Generosity)
                + HeroTools.GetTraitLevel(ThisPlot.ParentFaction.Leader, HeroTools.HeroTraits.Mercy);
            double valorFactor
                = Math.Pow((ThisPlot.TotalStrength / (ThisPlot.ParentFaction.TotalStrength - ThisPlot.TotalStrength)),
                HeroTools.GetTraitLevel(ThisPlot.Leader, HeroTools.HeroTraits.Valor) == 0 ? 1 : 2 * HeroTools.GetTraitLevel(ThisPlot.Leader, HeroTools.HeroTraits.Valor));
            double warPartyFactor
                = Math.Pow((ThisPlot.WarParties / (ThisPlot.ParentFaction.WarParties.Count() - ThisPlot.WarParties)),
                1 + HeroTools.GetTraitLevel(ThisPlot.Leader, HeroTools.HeroTraits.Calc));

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
            int honorScore = -(HeroTools.GetTraitLevel(hero, HeroTools.HeroTraits.Honor) + HeroTools.GetTraitLevel(hero.Clan.Kingdom.Leader, HeroTools.HeroTraits.Honor));
            int plottingFriends = HeroTools.GetPlottingFriends(hero, ThisPlot).Count();
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends);
            return plottingChance > tick;
        }

        public override bool DoPlot(Hero hero)
        {
            if (HeroTools.IsClanLeader(hero) && CanPlot(hero) && WantPlot(hero)) 
            {
                if (ThisPlot != null) { return ThisPlot.AddMember(hero); }
                return false; 
            }
            return false;
        }

        public override bool IsNewLeader(Hero hero)
        {
            if (ThisPlot.Leader is null) { ThisPlot.Leader = hero; return true; }
            else if (hero.Clan.Tier >= this.ThisPlot.Leader.Clan.Tier && HeroTools.GetPlottingFriends(hero, ThisPlot).Count() > HeroTools.GetPlottingFriends(ThisPlot.Leader, ThisPlot).Count())
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

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
        private static readonly MySettings settings = MySettings.Instance;

        public RecruitforCivilWarB()
        {

        }

        public override bool EndCondition(Plot plot)
        {
            int tick = new Random().Next(100);
            double warPersonality 
                = HeroTools.GetTraitLevel(plot.Leader, HeroTools.HeroTraits.Generosity)
                + HeroTools.GetTraitLevel(plot.Leader, HeroTools.HeroTraits.Mercy)
                + HeroTools.GetTraitLevel(plot.ParentFaction.Leader, HeroTools.HeroTraits.Generosity)
                + HeroTools.GetTraitLevel(plot.ParentFaction.Leader, HeroTools.HeroTraits.Mercy);
            double valorFactor
                = Math.Pow((plot.TotalStrength / (plot.ParentFaction.TotalStrength - plot.TotalStrength)),
                HeroTools.GetTraitLevel(plot.Leader, HeroTools.HeroTraits.Valor) == 0 ? 1 : 2 * HeroTools.GetTraitLevel(plot.Leader, HeroTools.HeroTraits.Valor));
            double warPartyFactor
                = Math.Pow((plot.WarParties / (plot.ParentFaction.WarParties.Count() - plot.WarParties)),
                1 + HeroTools.GetTraitLevel(plot.Leader, HeroTools.HeroTraits.Calc));

            return tick < settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;
        }

        public override bool EndResult(Plot plot)
        {
            Debug.PrintMessage("END OF PLOTTING REACHED\nEND OF PLOTTING REACHED\nEND OF PLOTTING REACHED\n");
            return true;
        }

        public override bool OnDailyTick(Plot plot)
        {
            foreach (Hero hero in plot.ParentFaction.Heroes)
            {
                DoPlot(hero, plot);
            }

            foreach (Hero plotter in plot.ClanLeaders)
            {
                LeaveCondition(plotter, plot);
                IsNewLeader(plotter, plot);
            }

            if (EndCondition(plot) && !plot.CurrentGoal.IsEndGoal)
            {
                plot.CurrentGoal = plot.CurrentGoal.NextGoal;
            }
            else { EndResult(plot); }
            return true;
        }

        public override bool CanPlot(Hero hero, Plot plot)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero) 
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !plot.Members.Contains(hero));
        }

        public override bool WantPlot(Hero hero, Plot plot)
        {
            int tick = new Random().Next(100);
            int honorScore = -(HeroTools.GetTraitLevel(hero, HeroTools.HeroTraits.Honor) + HeroTools.GetTraitLevel(hero.Clan.Kingdom.Leader, HeroTools.HeroTraits.Honor));
            int plottingFriends = HeroTools.GetPlottingFriends(hero, plot).Count();
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends);
            return plottingChance > tick;
        }

        public override bool DoPlot(Hero hero, Plot plot)
        {
            if (HeroTools.IsClanLeader(hero) && CanPlot(hero, plot) && WantPlot(hero, plot)) 
            {
                if (plot != null) { return plot.AddMember(hero); }
                return false; 
            }
            return false;
        }

        public override bool IsNewLeader(Hero hero, Plot plot)
        {
            if (plot.Leader is null) { plot.Leader = hero; return true; }
            else if (hero.Clan.Tier >= plot.Leader.Clan.Tier && HeroTools.GetPlottingFriends(hero, plot).Count() > HeroTools.GetPlottingFriends(plot.Leader, plot).Count())
            {
                plot.Leader = hero;
                return true;
            }
            return false;
        }

        public override bool LeaveCondition(Hero hero, Plot plot)
        {
            if ((hero.GetRelation(plot.ParentFaction.Leader) > settings.PositiveRelationThreshold
                && hero.GetRelation(plot.ParentFaction.Leader) > hero.GetRelation(plot.Leader)))
            {
                return plot.RemoveMember(hero);
            }
            return false;
        }
    }
}

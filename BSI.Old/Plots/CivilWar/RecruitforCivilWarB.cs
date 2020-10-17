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
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapNotificationTypes;
using TaleWorlds.Core.ViewModelCollection;

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
            double warchance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;

            Debug.AddEntry("War Chance in " + plot.TargetFaction.Name.ToString() + " || " + warchance.ToString());

            return tick < warchance;
                
        }

        public override bool EndResult(Plot plot)
        {
            Debug.PrintMessage("END OF PLOTTING REACHED\nEND OF PLOTTING REACHED\nEND OF PLOTTING REACHED\n");
            Debug.AddEntry("\nEND OF PLOTTING REACHED in " + plot.TargetFaction.Name.ToString());

            TextObject name = KingdomTools.NameGenerator(plot.Leader);
            TextObject informalname = new TextObject(plot.Leader.Clan.InformalName.ToString());

            Kingdom rebel = KingdomTools.CreateKingdom(plot.Leader, name, informalname, plot.Leader.Clan.Banner, true);

            foreach (Hero member in plot.ClanLeaders.Where(member => !member.Clan.Kingdom.Equals(rebel)))
            {
                ChangeKingdomAction.ApplyByJoinToKingdom(member.Clan, rebel, true);
            }
            DeclareWarAction.Apply(rebel, plot.TargetFaction);
            
            InformationManager.AddNotice(new WarMapNotification(rebel, plot.TargetFaction, new TextObject("Civil War breaks out in " + plot.TargetFaction.Name + "!!!")));

            Debug.AddEntry("Successful Revolt created " + rebel.Name.ToString());
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
                IsNewLeader(plotter, plot);
                LeaveCondition(plotter, plot);
            }

            if (EndCondition(plot) && !plot.CurrentGoal.IsEndGoal)
            {
                EndResult(plot);
                plot.NextGoal();
            }
            else { EndResult(plot); }
            return true;
        }

        public override bool CanPlot(Hero hero, Plot plot)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero) 
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !plot.Members.Contains(hero) 
                && !hero.Clan.IsMinorFaction);
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
                foreach (Hero member in hero.Clan.Lords)
                {
                    plot.AddMember(member);
                }
                return true;
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
            if (HeroTools.IsClanLeader(hero) && (hero.GetRelation(plot.ParentFaction.Leader) > settings.PositiveRelationThreshold
                && hero.GetRelation(plot.ParentFaction.Leader) > hero.GetRelation(plot.Leader)))
            {
                List<Hero> leavers = new List<Hero>();
                foreach (Hero member in plot.Members.Where(member => member.Clan.Equals(hero.Clan)))
                {
                   leavers.Add(member);
                }
                foreach (Hero leaver in leavers)
                {
                    plot.RemoveMember(leaver);
                }
                return !plot.Members.Contains(hero);
            }
            return false;
        }
    }
}

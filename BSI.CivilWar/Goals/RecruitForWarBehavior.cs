using BSI.Core;
using BSI.Core.Enumerations;
using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
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
                = HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotLeader(plot), CharacterTrait.Generosity)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotLeader(plot), CharacterTrait.Mercy)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Generosity)
                + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Mercy);
            double valorFactor
                = Math.Pow((plot.TotalStrength / (HeroExtension.ConvertToHero.TotalStrength - plot.TotalStrength)),
                HeroExtension.GetCharacterTraitLevel(plot.Leader, HeroExtension.CharacterTrait.Valor) == 0 ? 1 : 2 * HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotTargetLeaderHero(plot), CharacterTrait.Valor));
            double warPartyFactor
                = Math.Pow((plot.WarParties / (plot.ParentFaction.WarParties.Count() - plot.WarParties)),
                1 + HeroExtension.GetCharacterTraitLevel(HeroExtension.GetPlotLeaderHero(plot), CharacterTrait.Calculating));
            double warchance = settings.WarBaseChance * Math.Pow(settings.WarPersonalityMult, warPersonality) * valorFactor * warPartyFactor;

            Debug.AddEntry("War Chance in " + HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom.Name.ToString() + " || " + warchance.ToString());

            return tick < warchance;
        }

        public override bool DoEnd(Plot plot)
        {
            TextObject name = BSI.Core.Managers.KingdomManager.NameGenerator(HeroExtension.GetPlotLeader(plot));
            TextObject informalname = new TextObject(HeroExtension.GetPlotLeader(plot).Clan.InformalName.ToString());
            Culture culture = HeroExtension.GetCultureCode(HeroExtension.GetPlotLeader(plot));

            Kingdom rebel = Core.Managers.KingdomManager.CreateKingdom(HeroExtension.ConvertToHero(plot.Leader), name, informalname, HeroExtension.GetPlotLeader(plot).Clan.Banner, true);

            foreach (MBObjectBase member in plot.Members)
            {
                if (!HeroExtension.ConvertToHero(member).Clan.Kingdom.Equals(rebel))
                {
                    ChangeKingdomAction.ApplyByJoinToKingdom(HeroExtension.ConvertToHero(member).Clan, rebel, true);
                }
                
            }
            Kingdom oldKingdom = HeroExtension.ConvertToHero(plot.Target).Clan.Kingdom;

            DeclareWarAction.Apply(rebel, oldKingdom);

            InformationManager.AddNotice(new WarMapNotification(rebel, oldKingdom, new TextObject("Civil War breaks out in " + oldKingdom.Name + "!!!")));

            Debug.AddEntry("Successful Revolt created: " + rebel.Name.ToString());

            return true;
        }

        public override void OnDailyTick(Plot plot)
        {
            foreach (Hero hero in plot.Parent.Heroes)
            {
                DoPlot(hero, plot);
            }

            foreach (Hero plotter in plot.ClanLeaders)
            {
                IsNewLeader(plotter, plot);
                LeaveCondition(plotter, plot);
            }

            if (CanEnd(plot) && !plot.CurrentGoal)
            {
                DoEnd(plot);
                plot.NextGoal();
            }
            else { EndResult(plot); }
            return true;
        }

        private bool CanPlot(Hero hero, Plot plot)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero)
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && !plot.Members.Contains(hero)
                && !hero.Clan.IsMinorFaction);
        }

        private bool WantPlot(Hero hero, Plot plot)
        {
            int tick = new Random().Next(100);
            int honorScore = -(HeroExtension.GetCharacterTraitLevel(hero, CharacterTrait.Honor) + HeroExtension.GetCharacterTraitLevel(hero.Clan.Kingdom.Leader, CharacterTrait.Honor));
            List<Hero> plottingFriends = HeroExtension.GetPlottingFriends(hero, plot);
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore) * Math.Pow(settings.PlotFriendMult, plottingFriends.Count);
            return plottingChance > tick;
        }

        private bool DoPlot(Hero hero, Plot plot)
        {
            if (HeroExtension.IsClanLeader(hero) && CanPlot(hero, plot) && WantPlot(hero, plot))
            {
                foreach (Hero member in hero.Clan.Lords)
                {
                    plot.Members.Add(member);
                }
                return true;
            }
            return false;
        }

        public bool IsNewLeader(Hero hero, Plot plot)
        {
            Hero plotLeader = HeroExtension.GetPlotLeader(plot);

            if (plot.Leader is null) { plot.Leader = hero; return true; }
            else if (hero.Clan.Tier >= plotLeader.Clan.Tier && HeroExtension.GetPlottingFriends(hero, plot).Count > HeroExtension.GetPlottingFriends(plotLeader, plot).Count)
            {
                plot.Leader = hero;
                return true;
            }
            return false;
        }

        public bool LeaveCondition(Hero hero, Plot plot)
        {
            if (HeroExtension.IsClanLeader(hero) && (hero.GetRelation(HeroExtension.ConvertToHero(plot.Target)) > settings.PositiveRelationThreshold
                && hero.GetRelation(HeroExtension.ConvertToHero(plot.Target)) > hero.GetRelation(HeroExtension.ConvertToHero(plot.Leader))))
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
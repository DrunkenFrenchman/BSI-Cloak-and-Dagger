using BSI.Core.Enumerations;
using BSI.Core.Objects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Extensions
{
    public static class HeroExtension
    {
        public static bool IsClanLeader(this Hero hero)
        {
            return hero.Clan?.Leader?.Equals(hero) ?? false;
        }

        public static bool IsFactionLeader(Hero hero)
        {
            return hero.Clan.Kingdom.Leader.Equals(hero);
        }

        public static bool IsPlotLeader(Hero hero, Plot plot)
        {
            return hero.Equals(plot.Leader);
        }

        public static List<Hero> GetPlottingFriends(this Hero hero, Plot plot)
        {
            var plottingFriends = new List<Hero>();
            foreach (Hero plotter in plot.Members)
            {
                if (hero.IsFriend(plotter))
                {
                    plottingFriends.Add(plotter);
                }
            }

            return plottingFriends;
        }

        public static Culture GetCultureCode(Hero hero)
        {
            Enumerations.Culture culture = (Enumerations.Culture)hero.Culture.GetCultureCode();
            return culture;
        }

        public static Hero GetPlotLeaderHero(Plot plot)
        {
            throw new NotImplementedException();
        }

        public static int GetCharacterTraitLevel(this Hero hero, CharacterTrait characterTrait)
        {
            return hero.GetTraitLevel(TraitObject.Find(characterTrait.ToString()));
        }

        public static Hero GetPlotLeader(Plot plot)
        {
            PlotLeaderTypes current = 0;
            if (plot.Leader.GetType().Equals(typeof(Kingdom))) { current = PlotLeaderTypes.Kingdom; }
            if (plot.Leader.GetType().Equals(typeof(Clan))) { current = PlotLeaderTypes.Clan; }
            if (plot.Leader.GetType().Equals(typeof(Hero))) { current = PlotLeaderTypes.Hero; }
            
            switch (current)
            {
                case PlotLeaderTypes.Kingdom:
                    Kingdom kingdom = (Kingdom)plot.Leader;
                    return kingdom.Leader;
                case PlotLeaderTypes.Clan:
                    Clan clan = (Clan)plot.Leader;
                    return clan.Leader;
                case PlotLeaderTypes.Hero:
                    return (Hero)plot.Leader;
                default:
                    throw new ArgumentException("Plot Leader not assigned");
            }
        }

        public static Hero GetPlotTargetLeaderHero(Plot plot)
        {
            PlotLeaderTypes current = 0;
            if (plot.Target.GetType().Equals(typeof(Kingdom))) { current = PlotLeaderTypes.Kingdom; }
            if (plot.Target.GetType().Equals(typeof(Clan))) { current = PlotLeaderTypes.Clan; }
            if (plot.Target.GetType().Equals(typeof(Hero))) { current = PlotLeaderTypes.Hero; }

            switch (current)
            {
                case PlotLeaderTypes.Kingdom:
                    Kingdom kingdom = (Kingdom)plot.Leader;
                    return kingdom.Leader;
                case PlotLeaderTypes.Clan:
                    Clan clan = (Clan)plot.Leader;
                    return clan.Leader;
                case PlotLeaderTypes.Hero:
                    return (Hero)plot.Leader;
                default:
                    throw new ArgumentException("Plot Leader not assigned");
            }
        }

        public static Hero ConvertToHero(MBObjectBase leader)
        {
            PlotLeaderTypes current = 0;
            if (leader.GetType().Equals(typeof(Kingdom))) { current = PlotLeaderTypes.Kingdom; }
            if (leader.GetType().Equals(typeof(Clan))) { current = PlotLeaderTypes.Clan; }
            if (leader.GetType().Equals(typeof(Hero))) { current = PlotLeaderTypes.Hero; }

            switch (current)
            {
                case PlotLeaderTypes.Kingdom:
                    Kingdom kingdom = (Kingdom)leader;
                    return kingdom.Leader;
                case PlotLeaderTypes.Clan:
                    Clan clan = (Clan)leader;
                    return clan.Leader;
                case PlotLeaderTypes.Hero:
                    return (Hero)leader;
                default:
                    throw new ArgumentException("Plot Leader not assigned");
            }
        }
    }
}
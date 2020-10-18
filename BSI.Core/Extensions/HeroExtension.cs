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


        public static int GetCharacterTraitLevel(this Hero hero, CharacterTrait characterTrait)
        {
            return hero.GetTraitLevel(TraitObject.Find(characterTrait.ToString()));
        }

        public static List<Hero> ConvertToHero(List<MBObjectBase> oldList)
        {
            List<Hero> newList = new List<Hero>();
            foreach (MBObjectBase mBObject in oldList)
            {
                PlotLeaderTypes current = 0;
                if (mBObject.GetType().Equals(typeof(Kingdom))) { current = PlotLeaderTypes.Kingdom; }
                if (mBObject.GetType().Equals(typeof(Clan))) { current = PlotLeaderTypes.Clan; }
                if (mBObject.GetType().Equals(typeof(Hero))) { current = PlotLeaderTypes.Hero; }

                switch (current)
                {
                    case PlotLeaderTypes.Kingdom:
                        Kingdom kingdom = (Kingdom)mBObject;
                        newList.AddRange(kingdom.Lords);
                        break;
                    case PlotLeaderTypes.Clan:
                        Clan clan = (Clan)mBObject;
                        newList.AddRange(clan.Lords);
                        break;
                    case PlotLeaderTypes.Hero:
                        Hero hero = (Hero)mBObject;
                        newList.Add(hero);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return newList;
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
    }
}
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Tools
{
    public static class HeroTools
    {
        public static bool IsClanLeader(Hero hero)
        {
            return hero.Clan.Leader.Equals(hero);
        }

        public static bool IsFactionLeader(Hero hero)
        {
            return hero.Clan.Kingdom.Leader.Equals(hero);
        }

        public static Hero GetHero(string stringId, IFaction faction = null, Plot plot = null)
        {
            try
            {
                if (!(faction is null))
                {
                    foreach (Hero hero in faction.Heroes)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
                if (!(plot is null))
                {
                    foreach (Hero hero in plot.Members)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
                else
                {
                    foreach (Hero hero in Hero.All)
                    {
                        if (hero.StringId.Equals(stringId)) { return hero; }
                    }
                }
            }
            catch { throw new ArgumentException(); }
            return null;
        }

        internal static List<TraitObject> traitObjects = new List<TraitObject>(TraitObject.All);
        public enum HeroTraits
        {
            Valor,
            Mercy,
            Honor,
            Generosity,
            Calc,
        }
        
        public static int GetTraitLevel(Hero hero, HeroTraits trait)
        {
            return hero.GetTraitLevel(TraitObject.Find(trait.ToString()));
        }

        public static List<Hero> GetPlottingFriends(Hero hero, Plot plot)
        {
            List<Hero> plottingFriends = new List<Hero>();
            foreach (Hero plotter in plot.Members)  
            {
                if (hero.IsFriend(plotter)) { plottingFriends.Add(plotter); }
            }

            return plottingFriends;
        }
    }      
}

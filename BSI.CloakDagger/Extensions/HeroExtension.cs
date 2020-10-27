using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.Extensions
{
    public static class HeroExtension
    {
        public static bool IsFactionLeader(this Hero hero)
        {
            return hero == hero.MapFaction?.Leader;
        }

        public static bool IsKingdomLeader(this Hero hero)
        {
            return hero == hero.Clan?.Kingdom?.Leader;
        }

        public static bool IsClanLeader(this Hero hero)
        {
            return hero == hero.Clan?.Leader;
        }

        public static bool IsPlotLeader(this Hero hero, Plot plot)
        {
            return hero == plot?.Leader;
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

        public static Culture GetCultureCode(this Hero hero)
        {
            return (Culture)hero.Culture.GetCultureCode();
        }


        public static int GetCharacterTraitLevel(this Hero hero, CharacterTrait characterTrait)
        {
            return hero.GetTraitLevel(TraitObject.Find(characterTrait.ToString()));
        }
    }
}
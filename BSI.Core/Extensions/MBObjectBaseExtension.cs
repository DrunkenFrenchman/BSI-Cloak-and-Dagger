using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Extensions
{
    public static class MBObjectBaseExtension
    {
        public static Kingdom ConvertToKingdom(this MBObjectBase gameObject)
        {
            if (gameObject is Kingdom kingdom)
            {
                return kingdom;
            }

            if (gameObject is Clan clan && clan.Kingdom != null)
            {
                return clan.Kingdom;
            }

            if (gameObject is Hero hero && hero.Clan?.Kingdom != null)
            {
                return hero.Clan.Kingdom;
            }

            if (gameObject is CharacterObject character && character.HeroObject?.Clan?.Kingdom != null)
            {
                return character.HeroObject.Clan.Kingdom;
            }

            return null;
        }

        public static Clan ConvertToClan(this MBObjectBase gameObject)
        {
            if (gameObject is Kingdom kingdom)
            {
                return kingdom.RulingClan;
            }

            if (gameObject is Clan clan)
            {
                return clan;
            }

            if (gameObject is Hero hero && hero.Clan != null)
            {
                return hero.Clan;
            }

            if (gameObject is CharacterObject character && character.HeroObject?.Clan != null)
            {
                return character.HeroObject.Clan;
            }

            return null;
        }

        public static Hero ConvertToHero(this MBObjectBase gameObject)
        {
            if (gameObject is Kingdom kingdom)
            {
                return kingdom.Leader;
            }

            if (gameObject is Clan clan)
            {
                return clan.Leader;
            }

            if (gameObject is Hero hero)
            {
                return hero;
            }

            if (gameObject is CharacterObject character && character.IsHero)
            {
                return character.HeroObject;
            }

            return null;
        }

        public static CharacterObject ConvertToCharacter(this MBObjectBase gameObject)
        {
            if (gameObject is Kingdom kingdom)
            {
                return kingdom.Leader.CharacterObject;
            }

            if (gameObject is Clan clan)
            {
                return clan.Leader.CharacterObject;
            }

            if (gameObject is Hero hero)
            {
                return hero.CharacterObject;
            }

            if (gameObject is CharacterObject character)
            {
                return character;
            }

            return null;
        }
    }
}
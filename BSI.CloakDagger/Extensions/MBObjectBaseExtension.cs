using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Extensions
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

        public static List<Kingdom> ConvertToKingdoms(this List<MBObjectBase> gameObjects)
        {
            var kingdoms = new List<Kingdom>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is Kingdom kingdom)
                {
                    kingdoms.Add(kingdom.ConvertToKingdom());
                }

                if (gameObject is Clan clan)
                {
                    kingdoms.Add(clan.ConvertToKingdom());
                }

                if (gameObject is Hero hero)
                {
                    kingdoms.Add(hero.ConvertToKingdom());
                }

                if (gameObject is CharacterObject character)
                {
                    kingdoms.Add(character.ConvertToKingdom());
                }
            }

            return kingdoms;
        }

        public static List<Clan> ConvertToClans(this List<MBObjectBase> gameObjects)
        {
            var clans = new List<Clan>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is Kingdom kingdom)
                {
                    clans.Add(kingdom.ConvertToClan());
                }

                if (gameObject is Clan clan)
                {
                    clans.Add(clan.ConvertToClan());
                }

                if (gameObject is Hero hero)
                {
                    clans.Add(hero.ConvertToClan());
                }

                if (gameObject is CharacterObject character)
                {
                    clans.Add(character.ConvertToClan());
                }
            }

            return clans;
        }

        public static List<Hero> ConvertToHeroes(this List<MBObjectBase> gameObjects)
        {
            var heroes = new List<Hero>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is Kingdom kingdom)
                {
                    heroes.Add(kingdom.ConvertToHero());
                }

                if (gameObject is Clan clan)
                {
                    heroes.Add(clan.ConvertToHero());
                }

                if (gameObject is Hero hero)
                {
                    heroes.Add(hero.ConvertToHero());
                }

                if (gameObject is CharacterObject character)
                {
                    heroes.Add(character.ConvertToHero());
                }
            }

            return heroes;
        }

        public static List<CharacterObject> ConvertToCharacters(this List<MBObjectBase> gameObjects)
        {
            var characters = new List<CharacterObject>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject is Kingdom kingdom)
                {
                    characters.Add(kingdom.ConvertToCharacter());
                }

                if (gameObject is Clan clan)
                {
                    characters.Add(clan.ConvertToCharacter());
                }

                if (gameObject is Hero hero)
                {
                    characters.Add(hero.ConvertToCharacter());
                }

                if (gameObject is CharacterObject character)
                {
                    characters.Add(character.ConvertToCharacter());
                }
            }

            return characters;
        }
    }
}
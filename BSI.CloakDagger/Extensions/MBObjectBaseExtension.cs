using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Extensions
{
    public static class MbObjectBaseExtension
    {
        public static Kingdom ToKingdom(this MBObjectBase gameObject)
        {
            switch (gameObject)
            {
                case Kingdom kingdom:
                    return kingdom;
                case Clan clan when clan.Kingdom != null:
                    return clan.Kingdom;
                case Hero hero when hero.Clan?.Kingdom != null:
                    return hero.Clan.Kingdom;
                case CharacterObject character when character.HeroObject?.Clan?.Kingdom != null:
                    return character.HeroObject.Clan.Kingdom;
                default:
                    return null;
            }
        }

        public static Clan ToClan(this MBObjectBase gameObject)
        {
            switch (gameObject)
            {
                case Kingdom kingdom:
                    return kingdom.RulingClan;
                case Clan clan:
                    return clan;
                case Hero hero when hero.Clan != null:
                    return hero.Clan;
                case CharacterObject character when character.HeroObject?.Clan != null:
                    return character.HeroObject.Clan;
                default:
                    return null;
            }
        }

        public static Hero ToHero(this MBObjectBase gameObject)
        {
            switch (gameObject)
            {
                case Kingdom kingdom:
                    return kingdom.Leader;
                case Clan clan:
                    return clan.Leader;
                case Hero hero:
                    return hero;
                case CharacterObject character when character.IsHero:
                    return character.HeroObject;
                default:
                    return null;
            }
        }

        public static CharacterObject ToCharacter(this MBObjectBase gameObject)
        {
            switch (gameObject)
            {
                case Kingdom kingdom:
                    return kingdom.Leader.CharacterObject;
                case Clan clan:
                    return clan.Leader.CharacterObject;
                case Hero hero:
                    return hero.CharacterObject;
                case CharacterObject character:
                    return character;
                default:
                    return null;
            }
        }

        public static List<Kingdom> ToKingdoms(this List<MBObjectBase> gameObjects)
        {
            var kingdoms = new List<Kingdom>();

            foreach (var gameObject in gameObjects)
            {
                switch (gameObject)
                {
                    case Kingdom kingdom:
                        kingdoms.Add(kingdom.ToKingdom());
                        break;
                    case Clan clan:
                        kingdoms.Add(clan.ToKingdom());
                        break;
                    case Hero hero:
                        kingdoms.Add(hero.ToKingdom());
                        break;
                    case CharacterObject character:
                        kingdoms.Add(character.ToKingdom());
                        break;
                }
            }

            return kingdoms;
        }

        public static List<Clan> ToClans(this List<MBObjectBase> gameObjects)
        {
            var clans = new List<Clan>();

            foreach (var gameObject in gameObjects)
            {
                switch (gameObject)
                {
                    case Kingdom kingdom:
                        clans.Add(kingdom.ToClan());
                        break;
                    case Clan clan:
                        clans.Add(clan.ToClan());
                        break;
                    case Hero hero:
                        clans.Add(hero.ToClan());
                        break;
                    case CharacterObject character:
                        clans.Add(character.ToClan());
                        break;
                }
            }

            return clans;
        }

        public static List<Hero> ToHeroes(this List<MBObjectBase> gameObjects)
        {
            var heroes = new List<Hero>();

            foreach (var gameObject in gameObjects)
            {
                switch (gameObject)
                {
                    case Kingdom kingdom:
                        heroes.Add(kingdom.ToHero());
                        break;
                    case Clan clan:
                        heroes.Add(clan.ToHero());
                        break;
                    case Hero hero:
                        heroes.Add(hero.ToHero());
                        break;
                    case CharacterObject character:
                        heroes.Add(character.ToHero());
                        break;
                }
            }

            return heroes;
        }

        public static List<CharacterObject> ToCharacters(this List<MBObjectBase> gameObjects)
        {
            var characters = new List<CharacterObject>();

            foreach (var gameObject in gameObjects)
            {
                switch (gameObject)
                {
                    case Kingdom kingdom:
                        characters.Add(kingdom.ToCharacter());
                        break;
                    case Clan clan:
                        characters.Add(clan.ToCharacter());
                        break;
                    case Hero hero:
                        characters.Add(hero.ToCharacter());
                        break;
                    case CharacterObject character:
                        characters.Add(character.ToCharacter());
                        break;
                }
            }

            return characters;
        }
    }
}
using System.Collections.Generic;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Extensions
{
    public static class MbObjectBaseExtension
    {
        public static GameObject ToGameObject(this MBObjectBase mbObject)
        {
            switch (mbObject)
            {
                case CultureObject culture:
                    return new GameObject
                    {
                        GameObjectType = GameObjectType.Culture,
                        StringId = culture.StringId
                    };
                case Kingdom kingdom:
                    return new GameObject
                    {
                        GameObjectType = GameObjectType.Kingdom,
                        StringId = kingdom.StringId
                    };
                case Clan clan:
                    return new GameObject
                    {
                        GameObjectType = GameObjectType.Clan,
                        StringId = clan.StringId
                    };
                case Hero hero:
                    return new GameObject
                    {
                        GameObjectType = GameObjectType.Hero,
                        StringId = hero.StringId
                    };
                case CharacterObject character:
                    return new GameObject
                    {
                        GameObjectType = GameObjectType.Character,
                        StringId = character.StringId
                    };
                default:
                    return null;
            }
        }

        public static CultureObject ToCulture(this MBObjectBase mbObject)
        {
            switch (mbObject)
            {
                case Kingdom kingdom:
                    return kingdom.Culture;
                case Clan clan:
                    return clan.Culture;
                case Hero hero:
                    return hero.Culture;
                case CharacterObject character:
                    return character.Culture;
                default:
                    return null;
            }
        }

        public static Kingdom ToKingdom(this MBObjectBase mbObject)
        {
            switch (mbObject)
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

        public static Clan ToClan(this MBObjectBase mbObject)
        {
            switch (mbObject)
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

        public static Hero ToHero(this MBObjectBase mbObject)
        {
            switch (mbObject)
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

        public static CharacterObject ToCharacter(this MBObjectBase mbObject)
        {
            switch (mbObject)
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

        public static List<GameObject> ToGameObjects(this List<MBObjectBase> mbObjects)
        {
            var gameObjects = new List<GameObject>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
                {
                    case CultureObject culture:
                        gameObjects.Add(culture.ToGameObject());
                        break;
                    case Kingdom kingdom:
                        gameObjects.Add(kingdom.ToGameObject());
                        break;
                    case Clan clan:
                        gameObjects.Add(clan.ToGameObject());
                        break;
                    case Hero hero:
                        gameObjects.Add(hero.ToGameObject());
                        break;
                    case CharacterObject character:
                        gameObjects.Add(character.ToGameObject());
                        break;
                }
            }

            return gameObjects;
        }

        public static List<CultureObject> ToCultures(this List<MBObjectBase> mbObjects)
        {
            var cultures = new List<CultureObject>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
                {
                    case CultureObject culture:
                        cultures.Add(culture.ToCulture());
                        break;
                    case Kingdom kingdom:
                        cultures.Add(kingdom.ToCulture());
                        break;
                    case Clan clan:
                        cultures.Add(clan.ToCulture());
                        break;
                    case Hero hero:
                        cultures.Add(hero.ToCulture());
                        break;
                    case CharacterObject character:
                        cultures.Add(character.ToCulture());
                        break;
                }
            }

            return cultures;
        }

        public static List<Kingdom> ToKingdoms(this List<MBObjectBase> mbObjects)
        {
            var kingdoms = new List<Kingdom>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
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

        public static List<Clan> ToClans(this List<MBObjectBase> mbObjects)
        {
            var clans = new List<Clan>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
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

        public static List<Hero> ToHeroes(this List<MBObjectBase> mbObjects)
        {
            var heroes = new List<Hero>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
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

        public static List<CharacterObject> ToCharacters(this List<MBObjectBase> mbObjects)
        {
            var characters = new List<CharacterObject>();

            foreach (var mbObject in mbObjects)
            {
                switch (mbObject)
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
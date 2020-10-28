﻿using System.Collections.Generic;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Extensions
{
    public static class GameObjectExtensions
    {
        public static MBObjectBase ToMbObject(this GameObject gameObject)
        {
            switch (gameObject.GameObjectType)
            {
                case GameObjectType.Culture:
                    return Campaign.Current.ObjectManager.GetObject<CultureObject>(gameObject.StringId);
                case GameObjectType.Kingdom:
                    return Campaign.Current.ObjectManager.GetObject<Kingdom>(gameObject.StringId);
                case GameObjectType.Clan:
                    return Campaign.Current.ObjectManager.GetObject<Clan>(gameObject.StringId);
                case GameObjectType.Hero:
                    return Campaign.Current.ObjectManager.GetObject<Hero>(gameObject.StringId);
                case GameObjectType.Character:
                    return Campaign.Current.ObjectManager.GetObject<CharacterObject>(gameObject.StringId);
                default:
                    return null;
            }
        }

        public static CultureObject ToCulture(this GameObject gameObject)
        {
            return gameObject.ToMbObject().ToCulture();
        }

        public static Kingdom ToKingdom(this GameObject gameObject)
        {
            return gameObject.ToMbObject().ToKingdom();
        }

        public static Clan ToClan(this GameObject gameObject)
        {
            return gameObject.ToMbObject().ToClan();
        }

        public static Hero ToHero(this GameObject gameObject)
        {
            return gameObject.ToMbObject().ToHero();
        }

        public static CharacterObject ToCharacter(this GameObject gameObject)
        {
            return gameObject.ToMbObject().ToCharacter();
        }

        public static List<MBObjectBase> ToMbObjects(this List<GameObject> gameObjects)
        {
            var baseGameObjects = new List<MBObjectBase>();

            foreach (var gameObject in gameObjects)
            {
                switch (gameObject.GameObjectType)
                {
                    case GameObjectType.Culture:
                        baseGameObjects.Add(Campaign.Current.ObjectManager.GetObject<CultureObject>(gameObject.StringId));
                        break;
                    case GameObjectType.Kingdom:
                        baseGameObjects.Add(Campaign.Current.ObjectManager.GetObject<Kingdom>(gameObject.StringId));
                        break;
                    case GameObjectType.Clan:
                        baseGameObjects.Add(Campaign.Current.ObjectManager.GetObject<Clan>(gameObject.StringId));
                        break;
                    case GameObjectType.Hero:
                        baseGameObjects.Add(Campaign.Current.ObjectManager.GetObject<Hero>(gameObject.StringId));
                        break;
                    case GameObjectType.Character:
                        baseGameObjects.Add(Campaign.Current.ObjectManager.GetObject<CharacterObject>(gameObject.StringId));
                        break;
                }
            }

            return baseGameObjects;
        }

        public static List<CultureObject> ToCultures(this List<GameObject> gameObjects)
        {
            return gameObjects.ToMbObjects().ToCultures();
        }

        public static List<Kingdom> ToKingdoms(this List<GameObject> gameObjects)
        {
            return gameObjects.ToMbObjects().ToKingdoms();
        }

        public static List<Clan> ToClans(this List<GameObject> gameObjects)
        {
            return gameObjects.ToMbObjects().ToClans();
        }

        public static List<Hero> ToHeroes(this List<GameObject> gameObjects)
        {
            return gameObjects.ToMbObjects().ToHeroes();
        }

        public static List<CharacterObject> ToCharacters(this List<GameObject> gameObjects)
        {
            return gameObjects.ToMbObjects().ToCharacters();
        }
    }
}
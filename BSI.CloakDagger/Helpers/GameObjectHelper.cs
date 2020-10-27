using System.Collections.Generic;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Helpers
{
    public static class GameObjectHelper
    {
        public static MBObjectBase GetMBObjectByGameObject(GameObject gameObject)
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

        public static List<MBObjectBase> GetMBObjectsByGameObjects(List<GameObject> gameObjects)
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
    }
}
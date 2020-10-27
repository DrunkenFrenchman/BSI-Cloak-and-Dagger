using System.Collections.Generic;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.Extensions
{
    public static class GameObjectExtensions
    {
        public static Kingdom ToKingdom(this GameObject gameObject)
        {
            return GameObjectHelper.GetMBObjectByGameObject(gameObject).ToKingdom();
        }

        public static Clan ToClan(this GameObject gameObject)
        {
            return GameObjectHelper.GetMBObjectByGameObject(gameObject).ToClan();
        }

        public static Hero ToHero(this GameObject gameObject)
        {
            return GameObjectHelper.GetMBObjectByGameObject(gameObject).ToHero();
        }

        public static CharacterObject ToCharacter(this GameObject gameObject)
        {
            return GameObjectHelper.GetMBObjectByGameObject(gameObject).ToCharacter();
        }

        public static List<Kingdom> ToKingdoms(this List<GameObject> gameObjects)
        {
            return GameObjectHelper.GetMBObjectsByGameObjects(gameObjects).ToKingdoms();
        }

        public static List<Clan> ToClans(this List<GameObject> gameObjects)
        {
            return GameObjectHelper.GetMBObjectsByGameObjects(gameObjects).ToClans();
        }

        public static List<Hero> ToHeroes(this List<GameObject> gameObjects)
        {
            return GameObjectHelper.GetMBObjectsByGameObjects(gameObjects).ToHeroes();
        }

        public static List<CharacterObject> ToCharacters(this List<GameObject> gameObjects)
        {
            return GameObjectHelper.GetMBObjectsByGameObjects(gameObjects).ToCharacters();
        }
    }
}
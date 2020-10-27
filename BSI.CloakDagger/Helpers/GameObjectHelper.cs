using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Helpers
{
    public static class GameObjectHelper
    {
        public static MBObjectBase GetGameObjectByStringId(string stringId)
        {
            return Campaign.Current.ObjectManager.GetObject<MBObjectBase>(stringId);
        }

        public static List<MBObjectBase> GetGameObjectsByStringIds(List<string> stringIds)
        {
            var gameObjects = new List<MBObjectBase>();
            foreach (var stringId in stringIds)
            {
                gameObjects.Add(Campaign.Current.ObjectManager.GetObject<MBObjectBase>(stringId));
            }

            return gameObjects;
        }
    }
}
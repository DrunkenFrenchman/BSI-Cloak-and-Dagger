using System.Collections.Generic;
using System.Linq;
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
            return stringIds.Select(stringId => Campaign.Current.ObjectManager.GetObject<MBObjectBase>(stringId)).ToList();
        }
    }
}
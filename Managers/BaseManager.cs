using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWars.Helpers
{
    public interface IBaseManager<InfoType, GameObject> where GameObject : MBObjectBase
    {
        HashSet<InfoType> Infos { get; set; }

        void Initialize();

        void RemoveInvalids();

        void RemoveDuplicates();

        InfoType Get(GameObject gameObject);

        InfoType Get(string id);

        void Remove(string id);

        GameObject GetGameObject(string id);

        GameObject GetGameObject(InfoType info);
    }
}

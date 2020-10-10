using BSICivilWars.Helpers;
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
    public interface IBaseManager<GameObject, InfoType> where GameObject : MBObjectBase where InfoType : IFactionInfo<IFaction>
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

using BSICivilWars.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWars.Helpers
{
    public class KingdomManager : IBaseManager<string, FactionInfo>
    {
        public string Get(FactionInfo gameObject)
        {
            throw new NotImplementedException();
        }

        FactionInfo IBaseManager<string, FactionInfo>.GetGameObject(string id)
        {
            throw new NotImplementedException();
        }

        FactionInfo IBaseManager<string, FactionInfo>.GetGameObject(string info)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> Infos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void New()
        {

        }

        public string Get(Kingdom gameObject)
        {
            throw new NotImplementedException();
        }

        public string Get(string id)
        {
            throw new NotImplementedException();
        }

        public Kingdom GetGameObject(string id)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveDuplicates()
        {
            throw new NotImplementedException();
        }

        public void RemoveInvalids()
        {
            throw new NotImplementedException();
        }


    }
}

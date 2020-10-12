using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TKey, TValue> : Dictionary<String, IBSIObjectBase>, IBaseManager<String, IBSIObjectBase>
    {
        ICollection<IBSIObjectBase> IBaseManager<string, IBSIObjectBase>.Values => this.Values;

        public void AddItem(string stringId, IBSIObjectBase factionInfo)
        {
            this.Add(stringId, factionInfo);
        }
    }
}

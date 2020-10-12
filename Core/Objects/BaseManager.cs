using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TValue> : List<TValue> where TValue : IBSIObjectBase
    {
        public void AddItem(IBSIObjectBase factionInfo, IBSIManagerBase subManager)
        {
            throw new NotImplementedException();
        }
    }
}

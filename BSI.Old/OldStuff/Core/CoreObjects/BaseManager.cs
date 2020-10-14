using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using BSI.Manager;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public abstract class BaseManager<TValue> : List<TValue> where TValue : IBSIObjectBase
    {
 
    }
}

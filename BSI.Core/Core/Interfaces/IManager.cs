using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IManager<TValue> : IBSIManagerBase where TValue : IBSIObjectBase
    { 

    }
}

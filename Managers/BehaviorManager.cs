using BSI.Core;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Actions;

namespace BSI.Managers
{
    static class BehaviorManager
    {
        private static readonly System.Reflection.MemberInfo[] Behaviors = typeof(Behavior).GetMembers();

        //public static bool OnDailyTick()
        //{
        //    foreach (MemberInfo m in Behaviors)
        //    {
                
        //    }
        //}

    }
}

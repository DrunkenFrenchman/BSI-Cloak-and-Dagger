using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets.BannerEditor;

namespace BSI.Core
{
    public abstract class Behavior
    {
        public System.Reflection.MemberInfo[] GetMembers()
        {
            return typeof(Behavior).GetMembers();
        }
        
        public abstract bool EndCondition();
        public abstract bool OnDailyTick();
        public abstract bool OnGameLoad();

    }
}

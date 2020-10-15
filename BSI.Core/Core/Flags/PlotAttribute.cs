using BSI.Core.Managers;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI.Core.Flags
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PlotTriggerAttribute : Attribute
    {
     
        public PlotTriggerAttribute()
        {
            
        }
        public static PlotTriggerAttribute[] GetAll()
        {
            PlotTriggerAttribute[] temp = (PlotTriggerAttribute[])GetCustomAttributes(typeof(TriggerBase), typeof(PlotTriggerAttribute));
            return temp;
        }
    }
}

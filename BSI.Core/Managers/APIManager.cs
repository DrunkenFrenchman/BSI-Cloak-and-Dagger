using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BSI.Core.Managers
{
    public static class APIManager
    {
        internal static Dictionary<String, Type> PlotTypes = new Dictionary<String, Type>();

        public static void LoadPlot(Plot plot)
        {
            PlotTypes.Add(plot.PlotType.ToString(), plot.PlotType);
        }
        public static void RunDailyChecks()
        {
            foreach (Type type in PlotTypes.Values)
            {
                 
            }
        }
    }
}

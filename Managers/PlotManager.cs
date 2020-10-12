using BSI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace BSI.Manager
{
    public class PlotManager : IBSIObjectBase
    {
        public PlotManager(Plot plot)
        {
            this.Plot = plot;
        }
        public Plot Plot { get => this.Plot; set => this.Plot = value; }
        public MBReadOnlyList<Plot> All { get => this.All; }
    }
}

using BSI.Core.Objects;
using System.Collections.Generic;

namespace BSI.CloakDagger
{
    internal class PlotManager
    {
        public PlotManager()
        {
            this.Plots = new List<Plot>();
        }

        internal List<Plot> Plots { get; private set; }

        public void AddPlot(Plot plot)
        {
            if(this.Plots.Contains(plot))
            {
                return;
            }

            this.Plots.Add(plot);
        }

        public void RemovePlot(Plot plot)
        {
            if (!this.Plots.Contains(plot))
            {
                return;
            }

            this.Plots.Remove(plot);
        }
    }
}
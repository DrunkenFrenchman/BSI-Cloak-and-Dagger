using BSI.CloakDagger.Objects;
using System.Collections.Generic;

namespace BSI.CloakDagger.Managers
{
    internal class PlotManager
    {
        internal List<Plot> Plots { get; private set; }

        internal bool IsPlotMapFaction { get; set; }

        public PlotManager()
        {
            Plots = new List<Plot>();
        }

        public void AddPlot(Plot plot)
        {
            if (Plots.Contains(plot))
            {
                return;
            }

            Plots.Add(plot);
        }

        public void RemovePlot(Plot plot)
        {
            Plots.Remove(plot);
        }
    }
}
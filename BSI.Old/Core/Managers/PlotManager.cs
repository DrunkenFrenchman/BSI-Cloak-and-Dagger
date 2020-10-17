using BSI.Core;
using BSI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using BSI.Core.Objects;
using System.Collections.ObjectModel;

namespace BSI.Core.Manager
{
    public class PlotManager
    { 
        public bool IsPlotFaction { get; set; }
        private List<Plot> FactionPlots { get; set; }
        public ReadOnlyCollection<Plot> CurrentPlots { get; internal set; }
        public PlotManager(bool isPlotFaction = false)
        {
            FactionPlots = new List<Plot>();
            CurrentPlots = new ReadOnlyCollection<Plot>(FactionPlots);
            IsPlotFaction = isPlotFaction;
        }
        public bool AddPlot(Plot plot)
        {
            if (!CurrentPlots.Contains(plot))
            {
                FactionPlots.Add(plot);
                this.CurrentPlots = new ReadOnlyCollection<Plot>(FactionPlots);
                return true;
            }
            return false;
        }

        public bool RemovePlot(Plot plot)
        {
            if (FactionPlots.Contains(plot))
            {
                FactionPlots.Remove(plot);
                this.CurrentPlots = new ReadOnlyCollection<Plot>(FactionPlots);
                return true;
            }
            return false;
        }

    }
}

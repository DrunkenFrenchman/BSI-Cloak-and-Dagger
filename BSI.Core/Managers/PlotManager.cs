﻿using BSI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace BSI.Manager
{
    public class PlotManager : IBSIManagerBase
    {
        internal static List<Plot> AllPlots = new List<Plot>();

        public PlotManager()
        {

        }

        public PlotManager(Game game)
        {

        }

        public List<Plot> FactionPlots { get => this.FactionPlots; }

    }
}
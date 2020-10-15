using Messages.FromClient.ToLobbyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSI.Core.Flags
{
    public enum AvailableGoals
    {
        Default,
        RecuitforCivilWar
    }

    public static class PlotDescription
    {
        public static string Get(AvailableGoals goal)
        {
            switch (goal)
            {
                case AvailableGoals.RecuitforCivilWar:
                    return PlotDescription.RecruitforCivilWar;
                default:
                    return PlotDescription.Default;
            }
        }

        private static readonly string Default = "Default Plot: Should now appear";
        private static readonly string RecruitforCivilWar = "Gathering Forces";
    }
}

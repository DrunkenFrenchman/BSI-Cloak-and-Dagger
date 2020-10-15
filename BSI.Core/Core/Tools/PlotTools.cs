using BSI.Core.Flags;
using BSI.Core.Managers;
using BSI.Core.Objects;
using BSI.Plots.CivilWar;
using BSI.Plots.Default;
using Messages.FromClient.ToLobbyServer;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Tools
{
    public class PlotTools
    {
       public void GetGoal(AvailableGoals option, IFaction target, out Goal goal)
       {
            switch (option)
            {
                case AvailableGoals.RecuitforCivilWar:
                    goal = new RecruitforCivilWarG(target);
                    break;
                default:
                    goal = new DefaultG(target);
                    break;
            }
       }
    }
}

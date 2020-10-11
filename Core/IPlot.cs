using BSI.CivilWar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.CivilWar.Core
{
    public interface IPlot : IFaction
    {
        IFaction ParentFaction { get; set; }
        bool IsCivilWar { get; set; }
        IBaseManager<String, Clan> Members { get; set; }
        IBaseManager<String, Clan> Opponents { get; set; }
        bool EndGoal { get; set; }
    }
}

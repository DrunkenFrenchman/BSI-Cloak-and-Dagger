using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar
{
    internal class CivilWarPlot : Plot
    {
        internal CivilWarPlot()
        {
            UniqueTo = UniqueTo.Kingdom;
            TriggerType = typeof(CivilWarTrigger);
        }
    }
}
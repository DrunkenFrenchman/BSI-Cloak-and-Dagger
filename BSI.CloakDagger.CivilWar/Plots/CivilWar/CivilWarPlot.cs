using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using System.Collections.Generic;
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

        public override void Initialize(MBObjectBase target, MBObjectBase leader, Goal startGoal, Goal endGoal)
        {
            base.Initialize(target, leader, startGoal, endGoal);

            Members = new List<MBObjectBase>
            {
                leader.ConvertToClan()
            };
        }
    }
}
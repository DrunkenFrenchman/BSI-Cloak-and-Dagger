using BSI.CloakDagger.Enumerations;
using System;
using System.Collections.Generic;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Plot
    {
        public Type TriggerType { get; set; }

        public MBObjectBase Target { get; set; }

        public MBObjectBase Leader { get; set; }

        public List<MBObjectBase> Members { get; set; }

        public Goal CurrentGoal { get; set; }

        public Goal EndGoal { get; set; }

        public UniqueTo UniqueTo { get; set; }

        public bool IsEndGoalReached() => CurrentGoal == EndGoal;

        public void Initialize(MBObjectBase target, MBObjectBase leader, Goal startGoal, Goal endGoal)
        {
            Target = target;
            Leader = leader;
            CurrentGoal = startGoal;
            EndGoal = endGoal;
            Members = new List<MBObjectBase>
            {
                leader
            };
        }
    }
}
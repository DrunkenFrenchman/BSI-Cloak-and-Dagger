using BSI.Core.Enumerations;
using System;
using System.Collections.Generic;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Objects
{
    public abstract class Plot
    {
        public Plot(MBObjectBase target, MBObjectBase leader, Goal currentGoal, Goal endGoal, UniqueTo uniqueTo)
        {
            this.Target = target;
            this.Leader = leader;
            this.CurrentGoal = currentGoal;
            this.EndGoal = endGoal;
            this.UniqueTo = uniqueTo;
            this.CurrentGoal.Plot = this;
        }

        public MBObjectBase Parent { get; internal set; }

        public Type TriggerType { get; set; }

        public MBObjectBase Target { get; set; }

        public MBObjectBase Leader { get; set; }

        public List<MBObjectBase> Members { get; set; }

        public Goal CurrentGoal { get; set; }

        public Goal EndGoal { get; set; }

        public UniqueTo UniqueTo { get; set; }

        public bool IsEndGoal() => this.CurrentGoal == this.EndGoal;
    }
}
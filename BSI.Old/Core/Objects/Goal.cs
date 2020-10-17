using BSI.Core.Flags;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Objects
{

    public abstract class Goal
    {
        public Goal(
            IFaction target,
            AvailableGoals nextGoal = 0
            )
        {
            this.Target = target;
            this.NextGoal = nextGoal;
        }

        public IFaction Target { get; internal set; }
        public Plot Plot { get; internal set; }
        public abstract Behavior Behavior { get; internal set; }
        public bool IsEndGoal { get => this.NextGoal.Equals(this); }
        public AvailableGoals NextGoal { get; internal set; }
        public virtual string Name { get => "Plot to " + this.Manifesto; }
        public bool EndCondition { get => this.Behavior.EndCondition(Plot); }
        public abstract string Manifesto { get; }


    }
}

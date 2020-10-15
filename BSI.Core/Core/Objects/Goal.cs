using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Objects
{

    public abstract class Goal
    {
        public Goal(
            IFaction target,
            Goal nextGoal = null
            )
        {
            this.Target = target;
            this.NextGoal = nextGoal is null ? this : nextGoal;
        }

        public IFaction Target { get; internal set; }
        public Plot Plot { get; internal set; }
        public abstract Behavior Behavior { get; internal set; }
        public bool IsEndGoal { get => this.NextGoal.Equals(this); }
        public Goal NextGoal { get; internal set; }
        public virtual string Name { get => this.Name; }
        public bool EndCondition { get => this.Behavior.EndCondition(Plot); }
        public virtual string Manifesto { get => this.Manifesto; }


    }
}

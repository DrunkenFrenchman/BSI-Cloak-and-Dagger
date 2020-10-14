using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
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

#pragma warning disable CS0628 // New protected member declared in sealed class
        public IFaction Target { get; internal set; }
        public Plot Plot { get; internal set; }
        protected abstract BehaviorCore Behavior { get; }
        public bool IsEndGoal { get => this.NextGoal.Equals(this); }
        public Goal NextGoal { get; internal set; }
        public virtual string Name { get => this.Name; }
        public bool EndCondition { get => this.Behavior.EndCondition(); }
        public virtual string Manifesto { get => this.Manifesto; }


    }
}

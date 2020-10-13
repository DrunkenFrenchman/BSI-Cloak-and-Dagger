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
        private IFaction Target { get => this.Target; set => this.Target = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        private Plot Plot { get => this.Plot; set => this.Plot = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
        protected abstract BehaviorCore Behavior { get; }
#pragma warning disable CS0628 // New protected member declared in sealed class
        public bool IsEndGoal { get => this.NextGoal.Equals(this); }
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected Goal NextGoal { get => this.NextGoal; set => this.NextGoal = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
        public virtual string Name { get => this.Name; }
        public bool EndCondition { get => this.Behavior.EndCondition(); }
        public virtual string Manifesto { get => this.Manifesto; }
        public IFaction GetTarget { get => this.Target; }
        public Plot GetPlot => this.Plot;
        public Goal GetNextGoal { get => this.NextGoal; }



    }
}

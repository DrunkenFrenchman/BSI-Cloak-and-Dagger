using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Goal
    {
        public Goal(MBObjectBase target, Behavior behavior, string manifesto = null)
        {
            this.Target = target;
            this.Behavior = behavior;
            this.Manifesto = string.IsNullOrEmpty(manifesto) ? $"Plot against {this.Target.GetName()}" : manifesto;
            this.NextPossibleGoals = new List<Goal>();
            this.IsActive = false;
        }

        public MBObjectBase Target { get; internal set; }

        public Behavior Behavior { get; internal set; }

        public string Manifesto { get; set; }

        public List<Goal> NextPossibleGoals { get; internal set; }

        public Plot Plot { get; internal set; }

        public TextObject Name => new TextObject(this.Manifesto);

        public bool IsActive { get; set; }

        public virtual void SetNextGoal(Type goalType)
        {
            if(this.Plot.IsEndGoalReached())
            {
                return;
            }

            this.Plot.CurrentGoal = this.NextPossibleGoals.FirstOrDefault(goal => goal.GetType() == goalType.GetType());
            this.Plot.CurrentGoal.Plot = this.Plot;
            this.Plot.CurrentGoal.IsActive = true;

            this.IsActive = false;
        }
    }
}
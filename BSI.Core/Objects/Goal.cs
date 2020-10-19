using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.Core.Objects
{
    public abstract class Goal
    {
        public Goal(MBObjectBase target, Behavior behavior, string manifesto = null, List < Goal> goals = null)
        {
            this.Target = target;
            this.Behavior = behavior;
            this.NextPossibleGoals = goals ?? new List<Goal>();
            this.Manifesto = string.IsNullOrEmpty(manifesto) ? $"Plot against {this.Target.GetName()}" : manifesto;
        }

        public Plot Plot { get; internal set; }

        public TextObject Name { get => new TextObject(this.Manifesto); }

        public MBObjectBase Target { get; internal set; }

        public Behavior Behavior { get; internal set; }

        public string Manifesto { get; set; }

        public List<Goal> NextPossibleGoals { get; internal set; }

        public virtual void SetNextGoal(Type goalType)
        {
            if(this.Plot.IsEndGoal())
            {
                return;
            }

            this.Plot.CurrentGoal = this.NextPossibleGoals.FirstOrDefault(goal => goal.GetType() == goalType.GetType());
            this.Plot.CurrentGoal.Plot = this.Plot;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Objects
{
    public abstract class Goal
    {
        public Goal(Plot plot, MBObjectBase target, Behavior behavior, string manifesto = null)
        {
            Plot = plot;
            Target = target;
            Behavior = behavior;
            Manifesto = string.IsNullOrEmpty(manifesto) ? $"Plot against {Target.GetName()}" : manifesto;
            NextPossibleGoals = new List<Goal>();
        }

        public MBObjectBase Target { get; internal set; }

        public Behavior Behavior { get; internal set; }

        public string Manifesto { get; set; }

        public List<Goal> NextPossibleGoals { get; internal set; }

        public Plot Plot { get; internal set; }

        public TextObject Name => new TextObject(Manifesto);

        public virtual void SetNextGoal(Type goalType)
        {
            if(Plot.IsEndGoalReached())
            {
                return;
            }

            Plot.CurrentGoal = NextPossibleGoals.FirstOrDefault(goal => goal.GetType() == goalType.GetType());
        }
    }
}
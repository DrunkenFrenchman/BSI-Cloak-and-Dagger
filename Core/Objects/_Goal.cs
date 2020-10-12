using System;
using System.Collections.Generic;

namespace BSI.Core
{
    public sealed class Goal
    {
        public IBehavior Behavior { get => this.Behavior; set => this.Behavior = value; }
        public bool IsEndGoal { get => this.IsEndGoal; set => this.IsEndGoal = value; } 
        public Goal NextGoal { get => this.NextGoal; set => this.NextGoal = value; }
        public string Name { get => this.Name; set => this.Name = value; }
        public bool EndCondition { get => this.EndCondition; set => this.EndCondition = value; }
        public string Manifesto { get => "Plot to " + this.Manifesto; set => this.Manifesto = value; }
    }
}

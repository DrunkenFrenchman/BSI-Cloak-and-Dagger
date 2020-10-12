using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    [Flags]
    public enum Targets
    {
        Hero,
        Clan,
        Kingdom,
        Plot,
    }

    public sealed class Goal
    {
        public Goal(
            IFaction target, 
            String Name, 
            String manifesto, 
            Behavior behavior, 
            bool isEndGoal = true, 
            Goal nextGoal = null
            )
        {
            this.Target = target;
            this.Behavior = behavior;
            this.Name = Name;
            this.Manifesto = manifesto;
            this.IsEndGoal = isEndGoal;
            this.NextGoal = nextGoal;

        }
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected IFaction Target { get => this.Target; set => this.Target = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected Behavior Behavior { get => this.Behavior; set => this.Behavior = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected bool IsEndGoal { get => this.IsEndGoal; set => this.IsEndGoal = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected Goal NextGoal { get => this.NextGoal; set => this.NextGoal = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected string Name { get => this.Name; set => this.Name = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected bool EndCondition { get => this.Behavior.EndCondition(); }
#pragma warning restore CS0628 // New protected member declared in sealed class
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected string Manifesto { get => "Plot to " + this.Manifesto; set => this.Manifesto = value; }
#pragma warning restore CS0628 // New protected member declared in sealed class
        public IFaction GetTarget { get => this.Target; }
        public Behavior GetBehavior { get => this.Behavior; }
        public bool GetIsEndGoal { get => this.IsEndGoal; }
        public Goal GetNextGoal { get => this.NextGoal; }
        public string GetName { get => this.Name; }
        public bool GetEndCondition { get => this.EndCondition; }
        public string GetManifesto { get => "Plot to " + this.Manifesto; }



    }
}

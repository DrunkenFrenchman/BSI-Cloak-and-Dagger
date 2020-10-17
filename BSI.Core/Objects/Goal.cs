using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.TwoDimension;

namespace BSI.Core.Objects
{
    public abstract class Goal
    {
        public Goal(MBObjectBase target, Behavior behavior, String manifesto = "Plot against ")
        {
            this.Target = target;
            this.Behavior = behavior;
            if (manifesto == "Plot against ") { Manifesto = "Plot against " + Target.GetName().ToString(); }
            else { this.Manifesto = manifesto; }
        }
        public TextObject Name { get => new TextObject("Plot to " + Manifesto); }
        public MBObjectBase Target { get; internal set; }
        public Behavior Behavior { get; internal set; }
        public abstract bool EndCondition { get; }
        public virtual String Manifesto { get; internal set; }
    }
}
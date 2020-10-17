using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceGoal : Goal
    {
        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior, string manifesto = "Plot against ") : base(target, behavior, manifesto)
        {

        }

        public override bool EndCondition => throw new NotImplementedException();

        public override string Manifesto => base.Manifesto;
    }
}

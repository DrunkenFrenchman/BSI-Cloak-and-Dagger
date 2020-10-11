using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;

namespace BSI.Core
{
    public class Condition
    {
        public bool GoalIsMet(Plot plot)
        {
            if (plot.EndGoal == Goal.Independence)
            {
                return plot.ParentFaction.Equals(plot.OriginalFaction);
            }

            throw new NotImplementedException();
        }
    }
}

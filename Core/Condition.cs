using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;

namespace BSI.Core
{
    public class Condition
    {
        public Goal GoalIsMet(Plot plot)
        {
           
            if (plot.EndGoal.Contains(Goal.Independence))
            {
                if (plot.OriginalFaction.Equals(plot.ParentFaction)) { return Goal.Independence; }                
            }

            return Goal.NotMet;
        }
    }
}

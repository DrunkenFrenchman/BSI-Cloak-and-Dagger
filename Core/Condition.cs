using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
           
            if (plot.EndGoal.Equals(Goal.Independence))
            {
                if (plot.OriginalFaction.Equals(plot.ParentFaction)) { return Goal.Independence; }                
            }

            return Goal.NotMet;
        }

        public string PlotManifesto(Plot plot)
        {
            string manifesto = "Plot to ";

            if (plot.EndGoal.Equals(Goal.Independence))
            {
               return manifesto += "declare Independence from " + plot.OriginalFaction.Name.ToString();
            }

            return "ERROR NO GOAL MANIFESTO FOUND";
        }
    }
}

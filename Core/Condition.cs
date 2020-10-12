namespace BSI.Core
{
    public sealed class Condition
    {
        internal Goal GoalIsMet(Plot plot)
        {
           
            if (plot.EndGoal.Equals(Goal.Independence))
            {
                if (plot.OriginalFaction.Equals(plot.ParentFaction)) { return Goal.Independence; }                
            }

            return Goal.NotMet;
        }

        internal string PlotManifesto(Plot plot)
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

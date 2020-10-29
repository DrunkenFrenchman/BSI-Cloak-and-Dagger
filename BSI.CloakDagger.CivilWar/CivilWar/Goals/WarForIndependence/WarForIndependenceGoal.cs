using BSI.CloakDagger.Models.PlotMod;

namespace BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence
{
    public class WarForIndependenceGoal : Goal
    {
        public override void Initialize(string title, string description, Goal nextGoal, Plot plot)
        {
            base.Initialize(title, description, nextGoal, plot);

            Behavior = new WarForIndependenceBehavior();
            Behavior.Initialize(this);
        }
    }
}
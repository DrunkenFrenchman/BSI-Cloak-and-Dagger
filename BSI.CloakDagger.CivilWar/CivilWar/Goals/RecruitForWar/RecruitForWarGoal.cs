using BSI.CloakDagger.Objects;

namespace BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar
{
    public class RecruitForWarGoal : Goal
    {
        public override void Initialize(string title, string description, Goal nextGoal, Plot plot)
        {
            base.Initialize(title, description, nextGoal, plot);

            Behavior = new RecruitForWarBehavior();
            Behavior.Initialize(this);
        }
    }
}
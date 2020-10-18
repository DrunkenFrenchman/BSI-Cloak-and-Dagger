using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceBehavior : Behavior
    {
        public override bool CanEnd(Plot plot)
        {
            return plot.CurrentGoal.EndCondition;
        }

        public override bool DoEnd(Plot plot)
        {
            var newName = BSI.Core.Managers.KingdomManager.NameGenerator(plot.Leader.ConvertToHero());
            plot.Leader.ConvertToKingdom().ChangeKingdomName(newName, plot.Leader.ConvertToClan().InformalName);

            return true;
        }

        public override void OnDailyTick(Plot plot)
        {
            throw new NotImplementedException();
        }
    }
}

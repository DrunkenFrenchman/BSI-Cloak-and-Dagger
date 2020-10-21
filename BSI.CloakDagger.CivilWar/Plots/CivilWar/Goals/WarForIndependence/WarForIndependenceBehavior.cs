using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence
{
    internal class WarForIndependenceBehavior : Behavior
    {
        public override void DailyTick()
        {
            if (Goal == null || Goal != Goal?.Plot?.CurrentGoal)
            {
                return;
            }
        }

        public override bool CanEnd()
        {
            var plot = Goal.Plot;

            var plotTargetMapFaction = plot.Target.ConvertToHero().MapFaction;
            var plotLeaderMapFaction = plot.Leader.ConvertToHero().MapFaction;

            return plotLeaderMapFaction != plotTargetMapFaction
               && !plotLeaderMapFaction.IsAtWarWith(plotTargetMapFaction);
        }

        public override bool DoEnd()
        {
            var plot = Goal.Plot;

            var newName = Managers.KingdomManager.NameGenerator(plot.Leader.ConvertToHero());
            plot.Leader.ConvertToKingdom().ChangeKingdomName(newName, plot.Leader.ConvertToClan().InformalName);

            InformationManager.AddNotice(new PeaceMapNotification(plot.Leader.ConvertToKingdom(), plot.Target.ConvertToKingdom(), new TextObject($"Civil War ended in {plot.Target.ConvertToKingdom().Name}")));

            return true;
        }
    }
}

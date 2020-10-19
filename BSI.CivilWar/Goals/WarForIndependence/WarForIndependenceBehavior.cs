using BSI.Core.Extensions;
using BSI.Core.Managers;
using BSI.Core.Objects;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    public class WarForIndependenceBehavior : Behavior
    {
        public override bool CanEnd(Plot plot)
        {
            var plotTargetMapFaction = plot.Target.ConvertToHero().MapFaction;
            var plotLeaderMapFaction = plot.Leader.ConvertToHero().MapFaction;

             return plotLeaderMapFaction != plotTargetMapFaction
                && !plotLeaderMapFaction.IsAtWarWith(plotTargetMapFaction);
        }

        public override bool DoEnd(Plot plot)
        {
            var newName = KingdomManager.NameGenerator(plot.Leader.ConvertToHero());
            plot.Leader.ConvertToKingdom().ChangeKingdomName(newName, plot.Leader.ConvertToClan().InformalName);

            InformationManager.AddNotice(new PeaceMapNotification(plot.Leader.ConvertToKingdom(), plot.Target.ConvertToKingdom(), new TextObject($"Civil War ended in {plot.Target.ConvertToKingdom().Name}")));

            return true;
        }

        public override void OnDailyTick(Plot plot)
        {

        }
    }
}

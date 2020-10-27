using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence
{
    public class WarForIndependenceBehavior : Behavior
    {
        internal void DailyTick()
        {
        }

        public override bool CanEnd()
        {
            var plot = Goal.Plot;

            var plotTargetMapFaction = plot.Target.ToHero().MapFaction;
            var plotLeaderMapFaction = plot.Leader.ToHero().MapFaction;

            return plotLeaderMapFaction != plotTargetMapFaction && !plotLeaderMapFaction.IsAtWarWith(plotTargetMapFaction);
        }

        public override void DoEnd()
        {
            var plot = Goal.Plot;

            var newName = KingdomHelper.GenerateName(plot.Leader.ToHero());
            plot.Leader.ToKingdom().ChangeKingdomName(newName, plot.Leader.ToClan().InformalName);

            InformationManager.AddNotice(new PeaceMapNotification(plot.Leader.ToKingdom(), plot.Target.ToKingdom(), new TextObject($"Civil War ended in {plot.Target.ToKingdom().Name}")));
        }
    }
}
using BSI.Core.Extensions;
using BSI.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace BSI.CivilWar.Goals
{
    public class RecruitForWarGoal : Goal
    {
        public RecruitForWarGoal(Kingdom target, Behavior behavior) : base(target, behavior)
        {
            
        }
        public RecruitForWarGoal(Kingdom target, Behavior behavior, List<Goal> goals) : base(target, behavior, goals)
        {
            
        }
        public override string Manifesto => base.Manifesto;

        public override bool EndCondition => !GetPlot.Leader.ConvertToHero().MapFaction.Equals(GetPlot.Target.ConvertToHero().MapFaction)
            && FactionManager.GetEnemyKingdoms(GetPlot.Leader.ConvertToKingdom()).Contains(GetPlot.Target.ConvertToKingdom());
    }
}

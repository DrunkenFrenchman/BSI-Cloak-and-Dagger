using BSI.Core.Extensions;
using BSI.Core.Objects;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

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

        public override bool EndCondition => !HeroExtension.ConvertToHero(GetPlot.Leader).MapFaction.Equals(HeroExtension.ConvertToHero(GetPlot.Target).MapFaction)
            && FactionManager.GetEnemyKingdoms(HeroExtension.ConvertToKingdom(GetPlot.Leader)).Contains(HeroExtension.ConvertToKingdom(GetPlot.Target));
    }
}

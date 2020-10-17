using BSI.Core.Objects;
using System.Collections.Generic;
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
        public override string Manifesto => base.Manifesto;

        public override bool EndCondition => throw new System.NotImplementedException();
    }
}

using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

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
                TextObject newName = BSI.Core.Managers.KingdomManager.NameGenerator(HeroExtension.ConvertToHero(plot.Leader));
                HeroExtension.ConvertToKingdom(plot.Leader).ChangeKingdomName(newName, HeroExtension.ConvertToClan(plot.Leader).InformalName);
                //Close Plot//
                return true;
        }

        public override void OnDailyTick(Plot plot)
        {
            throw new NotImplementedException();
        }
    }
}

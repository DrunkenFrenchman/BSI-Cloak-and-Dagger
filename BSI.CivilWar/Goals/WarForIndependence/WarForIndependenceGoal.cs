using BSI.Core.Extensions;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceGoal : Goal
    {
        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior) : base (target, behavior)
        {

        }

        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior, List<Goal> goals, string manifesto = "Plot against ") : base(target, behavior, goals, manifesto)
        {

        }

        public override bool EndCondition => !HeroExtension.ConvertToHero(this.GetPlot.Leader).MapFaction.Equals(HeroExtension.ConvertToHero(GetPlot.Target).MapFaction)
            && !HeroExtension.ConvertToHero(this.GetPlot.Leader).MapFaction.IsAtWarWith(HeroExtension.ConvertToHero(GetPlot.Target).MapFaction);

        public override string Manifesto => "War of Independence from " + HeroExtension.ConvertToKingdom(GetPlot.Target);
    }
}

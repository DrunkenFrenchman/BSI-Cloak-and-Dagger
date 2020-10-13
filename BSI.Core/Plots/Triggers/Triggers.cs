using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using BSI.Plots;
using BSI.Core.Tools;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using System.Runtime.CompilerServices;

namespace BSI.Core.Triggers
{
    public class CivilWarT
    {

        public static readonly MySettings settings = new MySettings();
        internal static bool CanPlot(Hero hero)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero)
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && hero.Clan.Leader.Equals(hero));
        }

        internal static bool WantPlot(Hero hero)
        {
            int tick = new Random().Next(100);
            int honorScore = -(BSI_Hero.GetTraitLevel(hero, BSI_Hero.HeroTraits.Honor) + BSI_Hero.GetTraitLevel(hero.Clan.Kingdom.Leader, BSI_Hero.HeroTraits.Honor));
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore);
            return plottingChance > tick;
        }

        internal static bool DoPlot(Hero hero)
        {
            if (CanPlot(hero) && WantPlot(hero))
            {
                return true;
            }

            return false;
        }

    }
}

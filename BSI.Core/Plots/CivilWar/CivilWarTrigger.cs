using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public class CivilWarTrigger : TriggerBase
    {

        private static readonly BSI.Core.MySettings settings = BSI.Core.MySettings.Instance;
        public override bool CanPlot(Hero hero)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero)
                && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.NegativeRelationThreshold)
                && hero.Clan.Leader.Equals(hero));
        }

        public override bool WantPlot(Hero hero)
        {
            int tick = new Random().Next(100);
            int honorScore = -(BSI_Hero.GetTraitLevel(hero, BSI_Hero.HeroTraits.Honor) + BSI_Hero.GetTraitLevel(hero.Clan.Kingdom.Leader, BSI_Hero.HeroTraits.Honor));
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore);
            return plottingChance > tick;
        }

        public override bool DoPlot(Hero hero)
        {
            if (CanPlot(hero) && WantPlot(hero))
            {
                BSI_Faction.GetKingdom(hero.Clan.Kingdom).PlotManager.FactionPlots.Add(new CivilWar(hero, new RecruitforCivilWarG(hero.Clan.Kingdom)));
                return true;
            }

            return false;
        }
    }
}

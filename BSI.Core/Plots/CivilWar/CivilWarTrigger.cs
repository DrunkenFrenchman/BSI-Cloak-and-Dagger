using BSI.Core;
using BSI.Core.Flags;
using BSI.Core.Managers;
using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{

    public class CivilWarTrigger : TriggerBase
    {
        public CivilWarTrigger()
        {
            Uniqueto = Uniqueto.Kingdom;
        }
        //public override ReadOnlyCollection<Goal> PotentialEndGoals { get; internal set; }
        public override Uniqueto Uniqueto { get; internal set ; }

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
            int honorScore = -(HeroTools.GetTraitLevel(hero, HeroTools.HeroTraits.Honor) + HeroTools.GetTraitLevel(hero.Clan.Kingdom.Leader, HeroTools.HeroTraits.Honor));
            double plottingChance = settings.BasePlotChance * Math.Pow(settings.PlotPersonalityMult, honorScore);
            return plottingChance > tick;
        }

        public override bool DoPlot(Hero hero)
        {
            if (CanPlot(hero) && WantPlot(hero))
            {
                GameManager.FactionManager[hero.Clan.Kingdom].AddPlot(new CivilWar(hero, new RecruitforCivilWarG(hero.Clan.Kingdom), hero.Clan.Kingdom));
                return true;
            }

            return false;
        }
    }
}

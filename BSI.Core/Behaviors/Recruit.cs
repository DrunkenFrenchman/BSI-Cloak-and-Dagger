using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core;
using BSI.Core.Tools;
using BSI.Manager;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Behaviors
{
    public sealed class Recruit : BehaviorCore
    {
        private static MySettings settings = new MySettings();

        private readonly Plot ThisPlot;
        public Recruit(Plot plot)
        {
            this.ThisPlot = plot;
        }

        public override bool EndCondition()
        {
            throw new NotImplementedException();
        }

        public override bool OnDailyTick()
        {
            foreach (Kingdom k in GameManager.Kingdoms)
            {
                foreach (Hero hero in k.Heroes)
                {
                    if (BSI_Hero.IsClanLeader(hero) && CanPlot(hero))
                    {
                        StartedPlotting(hero);
                    }
                }
            }
        }

        internal bool CanPlot(Hero hero)
        {
            return (!hero.Clan.Kingdom.Leader.Equals(hero) && (hero.GetRelation(hero.Clan.Kingdom.Leader) < settings.BasePlotChance));
        }

        internal bool StartedPlotting(Hero hero)
        {
            int tick = new Random().Next(100);
            int honorScore = -(BSI_Hero.GetTraitLevel(hero, BSI_Hero.HeroTraits.Honor) + BSI_Hero.GetTraitLevel(hero.Clan.Kingdom.Leader, BSI_Hero.HeroTraits.Honor));
            int plottingFriends = BSI_Hero.GetPlottingFriends(hero, ThisPlot).Count();
        }

        public override bool OnGameLoad()
        {
            throw new NotImplementedException();
        }
    }
}

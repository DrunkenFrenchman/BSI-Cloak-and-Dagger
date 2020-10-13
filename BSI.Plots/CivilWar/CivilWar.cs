using BSI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using BSI.Manager;
using BSI.Core.Tools;

namespace BSI.Plots
{
    public class CivilWar : Plot, IPlot
    {
        
        public CivilWar(Hero instigator, Goal endGoal, Type type, Goal initialGoal = null) : base(instigator, endGoal, type, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                this.PlotType = type;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
            }
            else throw new ArgumentException();

        }

        protected class StageOne : Behavior
        {

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
                        if (BSI_Hero.IsClanLeader(hero))
                        {

                        }
                    }
                }
            }

            internal bool StartedPlotting(Hero hero)
            {
                
            }

            public override bool OnGameLoad()
            {
                throw new NotImplementedException();
            }
        }
    }
}

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
using TaleWorlds.CampaignSystem.Conversation.Tags;

namespace BSI.Plots
{
    public class CivilWar : Plot, IPlot
    {
        internal static MySettings settings = new MySettings();
        public CivilWar(Hero instigator, Goal endGoal, Type type, Goal initialGoal = null, Uniqueto uniqueto = Uniqueto.Kingdom) : base(instigator, endGoal, type, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator) && !instigator.Clan.Kingdom.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                this.PlotType = type;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                this.Uniqueto = uniqueto;
            }
            else throw new ArgumentException();

        }

        public Uniqueto Uniqueto { get => this.Uniqueto; set => this.Uniqueto = value; }

    }
}

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
using BSI.Core.CoreObjects;

namespace BSI.Plots.CivilWar
{
    public class CivilWar : Plot, IPlot
    {
        internal static MySettings settings = new MySettings();
        public CivilWar(Hero instigator, Goal endGoal, Goal initialGoal = null, Uniqueto uniqueto = Uniqueto.Kingdom) : base(instigator, endGoal, initialGoal)
        {

            if (instigator.Clan.Leader.Equals(instigator) && !instigator.Clan.Kingdom.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                this.Uniqueto = uniqueto;
            }
            else throw new ArgumentException();

        }

        public CivilWar(bool ONLY_USE_FOR_API_LOAD = true) : base(ONLY_USE_FOR_API_LOAD)
        {

        }

        public static new readonly Trigger Trigger = new CivilWarT();
        public override Uniqueto Uniqueto { get => this.Uniqueto; internal set => this.Uniqueto = value; }

    }
}

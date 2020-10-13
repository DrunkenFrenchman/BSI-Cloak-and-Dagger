using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.CoreObjects
{
    public abstract class Trigger
    {
        public virtual bool DoPlot(Hero hero)
        {
            return false;
        }
    }
}

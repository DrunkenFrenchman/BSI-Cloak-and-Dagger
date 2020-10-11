using BSI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IPlot : IFaction
    {
        IFaction ParentFaction { get; set; }
        IFaction OriginalFaction { get; set; }
        bool IsCivilWar { get; set; }
        IBaseManager<String, Hero> Members { get; }
        IBaseManager<String, Hero> Opponents { get; }
        new Hero Leader { get; set; }
        List<Goal> EndGoal { get; set; }
        void AddMember(Hero hero);
        void RemoveMember(Hero hero);
        void New();
        void End();
    }
}

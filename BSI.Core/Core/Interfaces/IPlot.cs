using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IPlot : IFaction, IBSIObjectBase
    {
        bool PlayerInvited { get; set; }
        FactionInfo ParentFaction { get; set; }
        FactionInfo OriginalFaction { get; set; }
        Type PlotType { get; set; }
        new Hero Leader { get; set; }
        Goal EndGoal { get; set; }
        List<Hero> Members { get; }
        bool AddMember(Hero hero);
        bool RemoveMember(Hero hero);
        void End();
    }
}

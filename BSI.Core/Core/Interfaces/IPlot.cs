using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IPlot : IFaction, IBSIObjectBase
    {
        bool PlayerInvited { get; set; }
        FactionInfo ParentFaction { get; }
        FactionInfo OriginalFaction { get; }
        Type PlotType { get; }
        new Hero Leader { get; }
        Goal EndGoal { get; }
        List<Hero> Members { get; }
        bool AddMember(Hero hero);
        bool RemoveMember(Hero hero);
        void End();
    }
}

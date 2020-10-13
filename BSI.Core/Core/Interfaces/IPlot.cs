using System;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IPlot : IFaction, IBSIObjectBase
    {
        bool PlayerInvited { get; set; }
        IFaction ParentFaction { get; set; }
        IFaction OriginalFaction { get; set; }
        Type PlotType { get; set; }
        new Hero Leader { get; set; }
        Goal EndGoal { get; set; }
        bool AddMember(Hero hero);
        bool RemoveMember(Hero hero);
        void End();
    }
}

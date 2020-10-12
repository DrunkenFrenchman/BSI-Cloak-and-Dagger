using System;
using TaleWorlds.CampaignSystem;

namespace BSI.Core
{
    public interface IPlot : IFaction, IBSIObjectBase
    {
        bool PlayerInvited { get; set; }
        IFaction ParentFaction { get; set; }
        IFaction OriginalFaction { get; set; }
        bool IsCivilWar { get; set; }
        IBaseManager<String, FactionInfo<IFaction>> Members { get; }
        IBaseManager<String, FactionInfo<IFaction>> Opponents { get; }
        new Hero Leader { get; set; }
        Goal EndGoal { get; set; }
        void AddMember(Hero hero);
        void RemoveMember(Hero hero);
        void End();
    }
}

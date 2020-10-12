using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BSI.Core
{
    public interface IFactionInfo<IFaction>
    {
        string StringId { get; }
        Hero Leader { get; }
        IEnumerable<Hero> Lords { get; }
        IEnumerable<MobileParty> AllParties { get; }
        IEnumerable<MobileParty> WarParties { get; }
        bool IsBanditFaction { get; }
        bool IsMinorFaction { get; }
        bool IsKingdomFaction { get; }
        bool IsCivilWar { get; }
        bool IsClan { get; }
        bool IsOutlaw { get; }
        bool IsMapFaction { get; }
        IFaction MapFaction { get; }
        float TotalStrength { get; }
        IEnumerable<StanceLink> Stances { get; }
        int TributeWallet { get; }
        float MainHeroCrimeRating { get; }
        float DailyCrimeRatingChange { get; }
        float Aggressiveness { get; }
        bool IsEliminated { get; }
        IEnumerable<Hero> Heroes { get; }
        StatExplainer DailyCrimeRatingChangeExplained { get; }
        CampaignTime NotAttackableByPlayerUntilTime { get; }
        IEnumerable<Settlement> Settlements { get; }
        TextObject Name { get; }
        TextObject InformalName { get; }
        CultureObject Culture { get; }
        IEnumerable<Town> Fiefs { get; }
        Banner Banner { get; }

        StanceLink GetStanceWith(IFaction other);
        bool IsAtWarWith(IFaction other);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.Localization;
using System.Reflection;
using BSI.CivilWar.Interface;

namespace BSI.CivilWar.Manager
{
    public abstract class FactionInfo : IFactionInfo<IFaction>
    {
        protected FactionInfo(IFaction faction)
        {
            Faction = faction;
        }
        IFaction Faction { get; set; }
        public string StringId { get => Faction.StringId; }
        public Hero Leader { get => Faction.Leader; set => throw new NotImplementedException(); }
        public IEnumerable<MobileParty> WarParties { get => Faction.WarParties; }

        public IEnumerable<Hero> Lords { get => Faction.Lords; }

        public IEnumerable<MobileParty> AllParties { get => Faction.AllParties; }

        public bool IsBanditFaction { get => Faction.IsBanditFaction; }

        public bool IsMinorFaction { get => Faction.IsMinorFaction; }

        public bool IsKingdomFaction { get => Faction.IsKingdomFaction; }

        public bool IsClan { get => Faction.IsClan; }

        public bool IsOutlaw { get => Faction.IsOutlaw; }

        public bool IsMapFaction { get => Faction.IsMapFaction; }

        public IFaction MapFaction { get => Faction.MapFaction; }

        public float TotalStrength { get => Faction.TotalStrength; }

        public IEnumerable<StanceLink> Stances { get => Faction.Stances; }

        public int TributeWallet { get => Faction.TributeWallet; set => throw new NotImplementedException(); }

        public float MainHeroCrimeRating { get => Faction.MainHeroCrimeRating ; set => throw new NotImplementedException(); }

        public float DailyCrimeRatingChange { get => Faction.DailyCrimeRatingChange; }

        public float Aggressiveness { get => Faction.Aggressiveness; }

        public bool IsEliminated { get => Faction.IsEliminated; }

        public IEnumerable<Hero> Heroes { get => Faction.Heroes; }

        public StatExplainer DailyCrimeRatingChangeExplained { get => Faction.DailyCrimeRatingChangeExplained; }

        public CampaignTime NotAttackableByPlayerUntilTime { get => Faction.NotAttackableByPlayerUntilTime; set => throw new NotImplementedException(); }

        public IEnumerable<Settlement> Settlements { get => Faction.Settlements; }

        public TextObject Name { get => Faction.Name; }

        public TextObject InformalName { get => Faction.InformalName; }

        public CultureObject Culture { get => Faction.Culture; }

        public IEnumerable<Town> Fiefs { get => Faction.Fiefs; }

        public Banner Banner { get => Faction.Banner; }

        public abstract StanceLink GetStanceWith(IFaction other);

        public abstract bool IsAtWarWith(IFaction other);

    }
}


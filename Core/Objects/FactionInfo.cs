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
using BSI.Core;
using HarmonyLib;
using BSI.Manager;

namespace BSI.Core
{
    public class FactionInfo : IFactionInfo<IFaction>, IBSIObjectBase
    {

        public FactionInfo(IFaction faction, bool isPlot = false)
        {
            Faction = faction;
            this.IsPlot = isPlot;           
        }
        public FactionInfo(IPlot faction, bool isCivilWar = true)
        {
            Faction = (IFaction) faction;
            this.IsPlot = isCivilWar;
        }

        public IFaction Faction { get => this.Faction; set => Faction = value; }

        public string StringId { get => Faction.StringId; }

        public Hero Leader { get => Faction.Leader; }

        public IEnumerable<MobileParty> WarParties { get => Faction.WarParties; }
        
        private IEnumerable<Hero> GetWarPartyLeaders()
        {
            var warPartyLeaders = new List<Hero>();
            if (!(this.IsClan || this.IsKingdomFaction || this.IsPlot)) { return warPartyLeaders; }
            foreach (MobileParty warParty in this.WarParties)
            {
                warPartyLeaders.Add(warParty.LeaderHero);
            }
            return warPartyLeaders;
        }
        
        public IEnumerable<Hero> WarPartyLeaders => this.GetWarPartyLeaders();

        public IEnumerable<Hero> Lords { get => Faction.Lords; }

        public IEnumerable<MobileParty> AllParties { get => Faction.AllParties; }

        public bool IsBanditFaction { get => Faction.IsBanditFaction; }

        public bool IsMinorFaction { get => Faction.IsMinorFaction; }

        public bool IsKingdomFaction { get => Faction.IsKingdomFaction; }

        public bool IsPlot { get => this.IsPlot; set => IsPlot = value; }

        public bool IsClan { get => Faction.IsClan; }

        public bool IsOutlaw { get => Faction.IsOutlaw; }

        public bool IsMapFaction { get => Faction.IsMapFaction; }

        public IFaction MapFaction { get => Faction.MapFaction; }

        public float TotalStrength { get => Faction.TotalStrength; }

        public IEnumerable<StanceLink> Stances { get => Faction.Stances; }

        public int TributeWallet { get => Faction.TributeWallet; set => Faction.TributeWallet = value; }

        public float MainHeroCrimeRating { get => Faction.MainHeroCrimeRating ; set => Faction.MainHeroCrimeRating = value; }

        public float DailyCrimeRatingChange { get => Faction.DailyCrimeRatingChange; }

        public float Aggressiveness { get => Faction.Aggressiveness; }

        public bool IsEliminated { get => Faction.IsEliminated; }

        public IEnumerable<Hero> Heroes { get => Faction.Heroes; }

        public StatExplainer DailyCrimeRatingChangeExplained { get => Faction.DailyCrimeRatingChangeExplained; }

        public CampaignTime NotAttackableByPlayerUntilTime { get => Faction.NotAttackableByPlayerUntilTime; set => NotAttackableByPlayerUntilTime = value; }

        public IEnumerable<Settlement> Settlements { get => Faction.Settlements; }

        public TextObject Name { get => Faction.Name; }

        public TextObject InformalName { get => Faction.InformalName; }

        public CultureObject Culture { get => Faction.Culture; }

        public IEnumerable<Town> Fiefs { get => Faction.Fiefs; }

        public Banner Banner { get => Faction.Banner; }

        internal BaseManager<IBSIObjectBase> PopulateManager()
        { 
            BaseManager<IBSIObjectBase> temp = new ClanManager<IBSIObjectBase>();
            
            temp.AddRange(this.VassalManager);
            temp.AddRange(this.PlotManager);
            return temp;
        }
        public BaseManager<IBSIObjectBase> SubFactionManager => this.PopulateManager();

        public BaseManager<IBSIObjectBase> VassalManager => this.VassalManager;

        public BaseManager<IBSIObjectBase> PlotManager => this.PlotManager;

        public StanceLink GetStanceWith(IFaction other) { return Faction.GetStanceWith(other); }

        public bool IsAtWarWith(IFaction other) { return Faction.IsAtWarWith(other); }

    }
}


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
using BSI.Core.Tools;

namespace BSI.Core
{
    public class FactionInfo : IFactionInfo<IFaction>, IBSIObjectBase
    {
        

        public FactionInfo(IFaction faction, PlotManager plotManager = null, bool isPlot = false)
        {
            Debug.AddEntry("Starting FactionInfo for: " + faction.Name.ToString());
            Faction = faction;
            if (!(plotManager is null)) { PlotManager = plotManager; }
            else { PlotManager = new PlotManager(); }
            try { BSI_Faction.Lookup.Add(faction.StringId, this); }
            catch { BSI_Faction.Lookup[faction.StringId] = this; }
        }

        public IFaction Faction { get; internal set; }

        public string StringId { get => Faction.StringId; }

        public Hero Leader { get; set; }

        public IEnumerable<MobileParty> WarParties { get => Faction.WarParties; }
        
        private IEnumerable<Hero> GetWarPartyLeaders()
        {
            var warPartyLeaders = new List<Hero>();
            if (!(this.IsClan || this.IsKingdomFaction || this.PlotManager.IsPlotFaction)) { return warPartyLeaders; }
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

        public bool IsClan { get => Faction.IsClan; }

        public bool IsOutlaw { get => Faction.IsOutlaw; }

        public bool IsMapFaction { get => Faction.IsMapFaction; }

        public IFaction MapFaction { get => Faction.MapFaction; }

        public float TotalStrength { get => Faction.TotalStrength; }

        public IEnumerable<StanceLink> Stances { get => Faction.Stances; }

        public int TributeWallet { get => Faction.TributeWallet; }

        public float MainHeroCrimeRating { get => Faction.MainHeroCrimeRating; }
        public float DailyCrimeRatingChange { get => Faction.DailyCrimeRatingChange; }

        public float Aggressiveness { get => Faction.Aggressiveness; }

        public bool IsEliminated { get => Faction.IsEliminated; }

        public IEnumerable<Hero> Heroes { get => Faction.Heroes; }

        public StatExplainer DailyCrimeRatingChangeExplained { get => Faction.DailyCrimeRatingChangeExplained; }

        public CampaignTime NotAttackableByPlayerUntilTime { get => Faction.NotAttackableByPlayerUntilTime; }

        public IEnumerable<Settlement> Settlements { get => Faction.Settlements; }

        public TextObject Name { get => Faction.Name; }

        public TextObject InformalName { get => Faction.InformalName; }

        public CultureObject Culture { get => Faction.Culture; }

        public IEnumerable<Town> Fiefs { get => Faction.Fiefs; }

        public Banner Banner { get => Faction.Banner; }

        internal List<FactionInfo> PopulateVassalManager()
        {
            List<FactionInfo> temp = new List<FactionInfo>();
            if (!this.IsClan)
            {
                foreach (Hero hero in this.Heroes)
                {
                    if (BSI_Hero.IsClanLeader(hero))
                    {
                        temp.Add(new FactionInfo(hero.Clan));
                    }
                }
            }
            return temp;
        }

        public List<FactionInfo> VassalManager => this.PopulateVassalManager();

        public PlotManager PlotManager { get; set; }
        public bool IsAtWarWith(IFaction other) { return Faction.IsAtWarWith(other); }

        public StanceLink GetStanceWith(IFaction other) { return Faction.GetStanceWith(other); }
      
    }
}


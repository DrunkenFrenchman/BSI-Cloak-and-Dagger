﻿using System;
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

        public FactionInfo(IFaction faction, bool isPlot = false)
        {
            this.Faction = faction;
            this.IsPlot = false;
            this.PlotManager = new PlotManager();
            BSI_Faction.Lookup.Add( faction.StringId, this );
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
                return temp;
            }
            else return null;
        }

        public List<FactionInfo> VassalManager => this.PopulateVassalManager();

        public PlotManager PlotManager { get => this.PlotManager; set => this.PlotManager = value; }
        public bool IsAtWarWith(IFaction other) { return Faction.IsAtWarWith(other); }

        public StanceLink GetStanceWith(IFaction other) { return Faction.GetStanceWith(other); }
      
    }
}

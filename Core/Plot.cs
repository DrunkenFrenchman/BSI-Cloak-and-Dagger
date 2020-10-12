using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using static TaleWorlds.CampaignSystem.Hero;

namespace BSI.Core
{
    public abstract class Plot : IPlot
    {
        public void Populate(Hero instigator)
        {
            this.ParentFaction = instigator.MapFaction;
            this.OriginalFaction = instigator.MapFaction;
            this.Leader = instigator;
        }

        public IFaction ParentFaction { get => this.ParentFaction; set => this.ParentFaction = value; }
        public IFaction OriginalFaction { get => this.ParentFaction; set => this.ParentFaction = value; }
        public bool IsCivilWar { get => this.IsCivilWar; set => this.IsCivilWar = value; }
        public IBaseManager<string, FactionInfo<IFaction>> Members { get => this.Members; }
        public IBaseManager<string, FactionInfo<IFaction>> Opponents { get => this.Opponents; }
        public Goal EndGoal { get => this.EndGoal; set => EndGoal = value; }
        public TextObject Name => new TextObject(condition.PlotManifesto(this));

        public string StringId { get => this.StringId; set => StringId = value; }

        public TextObject InformalName => new TextObject("Plot for " + this.EndGoal.ToString());

        public CultureObject Culture => this.OriginalFaction.Culture;

        public uint Color => this.OriginalFaction.Color;

        public uint Color2 => this.OriginalFaction.Color2;

        public uint AlternativeColor => this.OriginalFaction.AlternativeColor;

        public uint AlternativeColor2 => this.OriginalFaction.AlternativeColor2;

        public CharacterObject BasicTroop => this.OriginalFaction.BasicTroop;

        public Hero Leader { get => this.Leader; set => Leader = value; }

        public Banner Banner => this.Leader.ClanBanner;

        private List<Settlement> GetSettlements()
        {
            List<Settlement> settlements = new List<Settlement>();
            foreach (var member in this.Members)
            {
                foreach (Settlement settlement in member.Value.Settlements)
                {
                    settlements.Add(settlement);
                }
            }
            return settlements;
        }
        public IEnumerable<Settlement> Settlements => this.GetSettlements();

        private List<Hero> GetLords()
        {
            List<Hero> lords = new List<Hero>();

            foreach (Hero hero in this.Heroes)
            {
                if (!hero.Rank.Equals(FactionRank.None)) { lords.Add(hero); } 
            }
            return lords;
        }
        public IEnumerable<Hero> Lords => this.GetLords();

        private List<Hero> GetHeroes()
        {
            List<Hero> heroes = new List<Hero>();

            foreach (KeyValuePair<String, FactionInfo<IFaction>> pair in this.Members)
            {
                foreach (Hero hero in pair.Value.Heroes)
                {
                    heroes.Add(hero);
                }
            }

            return heroes;
        }

        public IEnumerable<Hero> Heroes => this.GetHeroes();

        private IEnumerable<MobileParty> GetAllParties()
        {
            List<MobileParty> mobileParties = new List<MobileParty>();

            foreach (KeyValuePair<String, FactionInfo<IFaction>> pair in this.Members)
            {
                foreach (MobileParty mobile in pair.Value.AllParties)
                {
                    mobileParties.Add(mobile);
                }
            }
            return mobileParties;        
        }
        public IEnumerable<MobileParty> AllParties => this.GetAllParties();

        private IEnumerable<MobileParty> GetWarParties()
        {
            List<MobileParty> warParties = new List<MobileParty>();

            foreach (KeyValuePair<String, FactionInfo<IFaction>> pair in this.Members)
            {
                foreach (MobileParty mobile in pair.Value.WarParties)
                {
                    warParties.Add(mobile);
                }
            }
            return warParties;
        }

        public IEnumerable<MobileParty> WarParties => this.GetWarParties();

        public bool IsBanditFaction => this.OriginalFaction.IsBanditFaction;

        public bool IsMinorFaction => this.IsMinorFaction;

        public bool IsKingdomFaction => this.IsKingdomFaction;

        public bool IsClan => this.IsClan;

        public bool IsOutlaw => this.IsOutlaw;

        public bool IsMapFaction => this.IsMapFaction;

        public IFaction MapFaction => this.ParentFaction;

        private float GetTotalStrength()
        {
            float strength = 0;
            foreach (MobileParty party in this.WarParties)
            {
                strength += party.GetTotalStrengthWithFollowers();
            }
            return strength;
        }
        public float TotalStrength => this.GetTotalStrength();

        public IEnumerable<StanceLink> Stances => this.Stances;

        public int TributeWallet { get => this.TributeWallet; set => TributeWallet = value; }
        public float MainHeroCrimeRating { get => this.MainHeroCrimeRating; set => MainHeroCrimeRating = value; }

        public float DailyCrimeRatingChange => this.DailyCrimeRatingChange;

        public float Aggressiveness => this.Aggressiveness;

        public bool IsEliminated => this.Members.IsEmpty();

        public StatExplainer DailyCrimeRatingChangeExplained => throw new NotImplementedException();

        public CampaignTime NotAttackableByPlayerUntilTime { get => this.NotAttackableByPlayerUntilTime; set => NotAttackableByPlayerUntilTime = value; }

        public void AddMember(Hero hero)
        {
            FactionInfo<Clan> factionInfo = new FactionInfo<Clan>(hero.Clan);
            this.Members.AddItem(hero.Clan.StringId, factionInfo);
            this.Opponents.Remove(hero.Clan.StringId);
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public StanceLink GetStanceWith(IFaction other)
        {
            return other.GetStanceWith(this.Leader.Clan);
        }

        public bool IsAtWarWith(IFaction other)
        {
            return other.IsAtWarWith(this.Leader.Clan);
        }

        public void RemoveMember(Hero hero)
        {
            FactionInfo<Clan> factionInfo = new FactionInfo<Clan>(hero.Clan);
            this.Opponents.AddItem(hero.Clan.StringId, factionInfo);
            this.Members.Remove(hero.Clan.StringId);
        }

        private Condition condition = new Condition();
        public Goal IsComplete => condition.GoalIsMet(this);
        
        // Methods below not used
        public string EncyclopediaLink => throw new NotImplementedException();
        public string EncyclopediaLinkWithName => throw new NotImplementedException();
        public TextObject EncyclopediaText => throw new NotImplementedException();
        public Vec2 InitialPosition => throw new NotImplementedException();
        public IEnumerable<Town> Fiefs => throw new NotImplementedException();
        public uint LabelColor => throw new NotImplementedException();
        public Vec2 FactionMidPoint => throw new NotImplementedException();


    }
}

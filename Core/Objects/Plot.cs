using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using static TaleWorlds.CampaignSystem.Hero;

namespace BSI.Core
{
    public abstract class Plot : IPlot, IBSIObjectBase
    {
        public Plot(
            Hero instigator,
            Goal endGoal,
            Goal initialGoal = null, 
            bool isCivilWar = false
            )
        {
            if (instigator.Clan.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                if (isCivilWar is true)
                {
                    this.IsCivilWar = true;
                }
            }
            else throw new ArgumentException();
        }

        public IBehavior CurrentBehavior { get => this.CurrentBehavior; set => this.CurrentBehavior = value; }
        public Goal CurrentGoal { get => this.CurrentGoal; set => this.CurrentGoal = value; }
        public bool PlayerInvited { get => this.PlayerInvited; set => this.PlayerInvited = value; }
        public IFaction ParentFaction { get => this.ParentFaction; set => this.ParentFaction = value; }
        public IFaction OriginalFaction { get => this.ParentFaction; set => this.ParentFaction = value; }
        public bool IsCivilWar { get => this.IsCivilWar; set => this.IsCivilWar = value; }
        public Goal EndGoal { get => this.EndGoal; set => EndGoal = value; }
        public TextObject Name => new TextObject(this.EndGoal.GetManifesto);
        public List<Hero> Members { get => this.Members; }
        private List<Hero> GetClanLeaders()
        {
            List<Hero> clanLeaders = new List<Hero>();
            foreach (Hero member in this.Members)
            {
                if (member.Equals(member.Clan.Leader)) { clanLeaders.Add(member); }
            }
            return clanLeaders;
        }
        public List<Hero> ClanLeaders => this.GetClanLeaders();
        public List<Hero> Opponents { get => this.Opponents; }
        public string StringId { get => this.StringId; set => StringId = value; }

        public TextObject InformalName => new TextObject("Plot for " + this.EndGoal.GetManifesto);

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
            foreach (Hero member in this.Members)
            {
                if (member.Equals(member.Clan.Leader)) { settlements.AddRange(member.Clan.Settlements); }
            }
            return settlements;
        }
        public IEnumerable<Settlement> Settlements => this.GetSettlements();

        private IEnumerable<Hero> GetLords()
        {
            List<Hero> lords = new List<Hero>();
            foreach (Hero clanLeader in this.ClanLeaders)
            {
                lords.AddRange(clanLeader.Clan.Lords);
            }
            return lords;
        }
        public IEnumerable<Hero> Lords => this.GetLords();

        public IEnumerable<Hero> Heroes => this.Heroes;

        private IEnumerable<MobileParty> GetAllParties()
        {
            List<MobileParty> mobileParties = new List<MobileParty>();

            foreach (Hero hero in this.ClanLeaders)
            {
                mobileParties.AddRange(hero.Clan.AllParties);
            }
            return mobileParties;        
        }
        public IEnumerable<MobileParty> AllParties => this.GetAllParties();

        private IEnumerable<MobileParty> GetWarParties()
        {
            List<MobileParty> warParties = new List<MobileParty>();

            foreach (Hero hero in this.ClanLeaders)
            {
                warParties.AddRange(hero.Clan.WarParties);
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

        public bool AddMember(Hero clanLeader)
        {
            if (this.Members.Contains(clanLeader) || !(clanLeader.Clan.Leader.Equals(clanLeader))) { return false; }
            else 
            {
                List<Hero> newMembers = new List<Hero>(clanLeader.Clan.Lords);
                this.Members.AddRange(newMembers);
                foreach (Hero newMember in newMembers)
                {
                    this.Opponents.Remove(newMember);
                } 
                return true;
            }
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

        public bool RemoveMember(Hero clanLeader)
        {
            if (!this.Members.Contains(clanLeader) || !(clanLeader.Clan.Leader.Equals(clanLeader))) { return false; }
            else
            {
                List<Hero> leavers = new List<Hero>(clanLeader.Clan.Lords);
                this.Opponents.AddRange(leavers);
                foreach (Hero leaver in leavers)
                {
                    this.Members.Remove(leaver);
                }
                return true;
            }
        }
        public bool IsComplete => (this.CurrentGoal == this.EndGoal && this.EndGoal.GetEndCondition);

        string IFaction.EncyclopediaLink => throw new NotImplementedException();

        string IFaction.EncyclopediaLinkWithName => throw new NotImplementedException();

        TextObject IFaction.EncyclopediaText => throw new NotImplementedException();

        Vec2 IFaction.InitialPosition => throw new NotImplementedException();

        uint IFaction.LabelColor => throw new NotImplementedException();

        IEnumerable<Town> IFaction.Fiefs => throw new NotImplementedException();

        Vec2 IFaction.FactionMidPoint => throw new NotImplementedException();


    }
}

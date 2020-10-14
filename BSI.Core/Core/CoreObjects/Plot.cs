using BSI.Core.CoreObjects;
using BSI.Core.Tools;
using BSI.Manager;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
            Goal initialGoal,
            Goal endGoal = null,
            Uniqueto unique = 0
            )
        {
            if (instigator.Clan.Leader.Equals(instigator))
            {
                this.Leader = instigator;
                this.EndGoal = endGoal;
                if (initialGoal is null) { this.CurrentGoal = initialGoal; }
                else { this.CurrentGoal = this.EndGoal; }
                PlotManager.AllPlots.Add(this);

                BSI_Faction.Lookup.TryGetValue(instigator.Clan.Kingdom.StringId, out FactionInfo faction);
                this.OriginalFaction = faction;
                //UniqueTo = unique;

                this.OriginalFaction.PlotManager.FactionPlots.Add(this);
            }

            else throw new ArgumentException();
        }
        public virtual Uniqueto Uniqueto { get; internal set; }
        public virtual Trigger Trigger { get; }
        public virtual BehaviorCore CurrentBehavior { get; internal set; }
        public virtual Goal CurrentGoal { get; internal set; }
        public virtual bool PlayerInvited { get; set; }
        public virtual FactionInfo ParentFaction { get; internal set; }
        public virtual FactionInfo OriginalFaction { get; internal set; }
        public virtual Type PlotType { get; internal set; }
        public virtual Goal EndGoal { get; internal set; }
        public virtual TextObject Name => new TextObject(this.EndGoal.Manifesto);
        public virtual List<Hero> Members { get; }
        private List<Hero> GetClanLeaders()
        {
            List<Hero> clanLeaders = new List<Hero>();
            foreach (Hero member in this.Members)
            {
                if (member.Equals(member.Clan.Leader)) { clanLeaders.Add(member); }
            }
            return clanLeaders;
        }
        public virtual List<Hero> ClanLeaders => this.GetClanLeaders();
        public virtual List<Hero> Opponents { get => this.Opponents; }
        public virtual string StringId { get => this.OriginalFaction.StringId + "_" + this.PlotType.ToString(); }

        public virtual TextObject InformalName => new TextObject("Plot for " + this.EndGoal.Manifesto);

        public virtual CultureObject Culture => this.OriginalFaction.Culture;

        public virtual uint Color => this.OriginalFaction.Faction.Color;

        public virtual uint Color2 => this.OriginalFaction.Faction.Color2;

        public virtual uint AlternativeColor => this.OriginalFaction.Faction.AlternativeColor;

        public virtual uint AlternativeColor2 => this.OriginalFaction.Faction.AlternativeColor2;

        public virtual CharacterObject BasicTroop => this.OriginalFaction.Faction.BasicTroop;

        public virtual Hero Leader { get; set; }

        public virtual Banner Banner => this.Leader.ClanBanner;

        private List<Settlement> GetSettlements()
        {
            List<Settlement> settlements = new List<Settlement>();
            foreach (Hero member in this.Members)
            {
                if (member.Equals(member.Clan.Leader)) { settlements.AddRange(member.Clan.Settlements); }
            }
            return settlements;
        }
        public virtual IEnumerable<Settlement> Settlements => this.GetSettlements();

        private IEnumerable<Hero> GetLords()
        {
            List<Hero> lords = new List<Hero>();
            foreach (Hero clanLeader in this.ClanLeaders)
            {
                lords.AddRange(clanLeader.Clan.Lords);
            }
            return lords;
        }
        public virtual IEnumerable<Hero> Lords => this.GetLords();

        public virtual IEnumerable<Hero> Heroes => this.Heroes;

        private IEnumerable<MobileParty> GetAllParties()
        {
            List<MobileParty> mobileParties = new List<MobileParty>();

            foreach (Hero hero in this.ClanLeaders)
            {
                mobileParties.AddRange(hero.Clan.AllParties);
            }
            return mobileParties;        
        }
        public virtual IEnumerable<MobileParty> AllParties => this.GetAllParties();

        private IEnumerable<MobileParty> GetWarParties()
        {
            List<MobileParty> warParties = new List<MobileParty>();

            foreach (Hero hero in this.ClanLeaders)
            {
                warParties.AddRange(hero.Clan.WarParties);
            }
            return warParties;
        }

        public virtual IEnumerable<MobileParty> WarParties => this.GetWarParties();

        public virtual bool IsBanditFaction => this.OriginalFaction.IsBanditFaction;

        public virtual bool IsMinorFaction => this.IsMinorFaction;

        public virtual bool IsKingdomFaction => this.IsKingdomFaction;

        public virtual bool IsClan => this.IsClan;

        public virtual bool IsOutlaw => this.IsOutlaw;

        public virtual bool IsMapFaction => this.IsMapFaction;

        public virtual IFaction MapFaction => this.ParentFaction.MapFaction;

        private float GetTotalStrength()
        {
            float strength = 0;
            foreach (MobileParty party in this.WarParties)
            {
                strength += party.GetTotalStrengthWithFollowers();
            }
            return strength;
        }
        public virtual float TotalStrength => this.GetTotalStrength();

        public virtual IEnumerable<StanceLink> Stances => this.Stances;

        public virtual int TributeWallet { get; set; }
        public virtual float MainHeroCrimeRating { get; set; }

        public virtual float DailyCrimeRatingChange => this.DailyCrimeRatingChange;

        public virtual float Aggressiveness => this.ParentFaction.Aggressiveness;

        public virtual bool IsEliminated => this.IsEliminated;

        public virtual StatExplainer DailyCrimeRatingChangeExplained => throw new NotImplementedException();

        public virtual CampaignTime NotAttackableByPlayerUntilTime { get; set; }

        public virtual bool AddMember(Hero clanLeader)
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
        public virtual void End()
        {
            throw new NotImplementedException();
        }

        public virtual StanceLink GetStanceWith(IFaction other)
        {
            return other.GetStanceWith(this.Leader.Clan);
        }

        public virtual bool IsAtWarWith(IFaction other)
        {
            return other.IsAtWarWith(this.Leader.Clan);
        }

        public virtual bool RemoveMember(Hero clanLeader)
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
        public virtual bool IsComplete => (this.CurrentGoal == this.EndGoal && this.EndGoal.EndCondition);

        string IFaction.EncyclopediaLink => throw new NotImplementedException();

        string IFaction.EncyclopediaLinkWithName => throw new NotImplementedException();

        TextObject IFaction.EncyclopediaText => throw new NotImplementedException();

        Vec2 IFaction.InitialPosition => throw new NotImplementedException();

        uint IFaction.LabelColor => throw new NotImplementedException();

        IEnumerable<Town> IFaction.Fiefs => throw new NotImplementedException();

        Vec2 IFaction.FactionMidPoint => throw new NotImplementedException();


    }
}

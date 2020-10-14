using BSI.Core.Managers;
using BSI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using static TaleWorlds.CampaignSystem.Hero;

namespace BSI.Core.Objects
{
    public abstract class Plot
    {
        public abstract TriggerBase Trigger { get; }
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
                this.Uniqueto = unique;
                switch (unique)
                {
                    case Uniqueto.Clan:
                        this.TargetFaction = instigator.Clan;
                        BSIManager.GameManager[this.TargetFaction].AddPlot(this);
                        return;
                    case Uniqueto.Kingdom:
                        this.TargetFaction = instigator.Clan.Kingdom;
                        BSIManager.GameManager[this.TargetFaction].AddPlot(this);
                        return;
                    case Uniqueto.Global:
                        BSIManager.GlobalPlots.AddPlot(this);
                        return;
                }
                this.Name = this.EndGoal.Manifesto;
            }

            else throw new ArgumentException();
        }
        public virtual String Name { get; internal set; }
        public virtual Uniqueto Uniqueto { get; internal set; }
        public virtual bool PlayerInvited { get; set; }
        public virtual IFaction ParentFaction { get; internal set; }
        public virtual IFaction TargetFaction { get; internal set; }
        public virtual Type PlotType { get => this.GetType(); }
        public virtual Hero Leader { get; internal set; }
        public virtual Goal CurrentGoal { get; internal set; }
        public virtual Goal EndGoal { get; internal set; }
        private List<Hero> CurrentMembers { get; set; }
        public ReadOnlyCollection<Hero> Members { get; internal set; }
        private double GetStrength()
        {
            double temp = 0;
            foreach (MobileParty party in this.CurrentWarParties)
            {
                temp += party.GetTotalStrengthWithFollowers();
            }
            return temp;
        }
        public virtual double TotalStrength { get; internal set; }

        private void GetWarParties()
        {
            CurrentWarParties.Clear();

            foreach (Hero hero in this.ClanLeaders)
            {
                foreach (MobileParty party in hero.Clan.WarParties)
                {
                    this.CurrentWarParties.Add(party);
                }
            }
        }
        public int WarParties { get => this.CurrentWarParties.Count; }
        private List<MobileParty> CurrentWarParties { get; set; }
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
        public bool AddMember(Hero hero)
        {
            if (!this.Members.Contains(hero))
            {
                this.CurrentMembers.Add(hero);
                this.Members = new ReadOnlyCollection<Hero>(CurrentMembers);
                return true;
            }
            return false;
        }

        public bool RemoveMember(Hero hero)
        {
            if (this.Members.Contains(hero))
            {
                this.CurrentMembers.Remove(hero);
                this.Members = new ReadOnlyCollection<Hero>(CurrentMembers);
                return true;
            }
            return false;
        }
        public void End()
        {

        }
    }
}

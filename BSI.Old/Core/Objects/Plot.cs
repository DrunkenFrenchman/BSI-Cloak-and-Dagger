using BSI.Core.Flags;
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
        public Plot(
           Hero instigator,
           AvailableGoals endGoal,
           IFaction target,
           AvailableGoals initialGoal = 0,
           Uniqueto unique = 0
           )
        {
            if (instigator.Clan.Leader.Equals(instigator))
            {
                this.Leader = instigator;

                this.CurrentMembers = new List<Hero>();
                AddMember(instigator);

                this.EndGoal = endGoal;

                new PlotTools().GetGoal(endGoal, target, out Goal newGoal);
                this.CurrentGoal = newGoal;

                this.Uniqueto = unique;
                this.TargetFaction = target;
                this.ParentFaction = target;
                switch (unique)
                {
                    case Uniqueto.Clan:
                        GameManager.FactionManager[instigator.Clan].AddPlot(this);
                        break;
                    case Uniqueto.Kingdom:
                        GameManager.FactionManager[instigator.Clan.Kingdom].AddPlot(this);
                        break;
                    case Uniqueto.Global:
                        GameManager.GlobalPlots.AddPlot(this);
                        break;
                }
                Name = PlotDescription.Get(this.EndGoal);

                Debug.AddEntry("Plot created || " + this.Name + " || " + this.Leader.Name.ToString());
            }

            else throw new ArgumentException();
        }
        public virtual String Name { get; internal set; }
        public abstract TriggerBase Trigger { get; }
        public virtual Uniqueto Uniqueto { get; internal set; }
        public virtual bool PlayerInvited { get; set; }
        public virtual IFaction ParentFaction { get; internal set; }
        public virtual IFaction TargetFaction { get; internal set; }
        public virtual Type PlotType { get => this.GetType(); }
        public virtual Hero Leader { get; internal set; }
        public virtual Goal CurrentGoal { get; internal set; }
        public virtual AvailableGoals EndGoal { get; internal set; }
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
        public virtual double TotalStrength { get => this.GetStrength(); internal set => this.TotalStrength = value; }

        private List<MobileParty> GetWarParties()
        {
            List<MobileParty> temp = new List<MobileParty>();

            foreach (Hero hero in this.ClanLeaders)
            {
                foreach (MobileParty party in hero.Clan.WarParties)
                {
                    temp.Add(party);
                }
            }
            return temp;
        }
        public int WarParties { get => this.CurrentWarParties.Count; }
        private List<MobileParty> CurrentWarParties { get=> this.GetWarParties(); set => this.CurrentWarParties = value; }
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
            if (HeroTools.IsClanLeader(hero) && !this.CurrentMembers.Contains(hero))
            {
                foreach (Hero member in hero.Clan.Lords)
                {
                    CurrentMembers.Add(member);
                }

            }
            this.Members = new ReadOnlyCollection<Hero>(CurrentMembers);
            return true;
        }

        public bool RemoveMember(Hero hero)
        {
            if (HeroTools.IsClanLeader(hero) && this.CurrentMembers.Contains(hero))
            {
                foreach (Hero member in hero.Clan.Lords)
                {
                    CurrentMembers.Remove(member);
                }

            }
            this.Members = new ReadOnlyCollection<Hero>(CurrentMembers);
            return true;
        }
        public void End()
        {

        }
        
        public void NextGoal()
        {
            if (!CurrentGoal.IsEndGoal)
            {
                new PlotTools().GetGoal(this.CurrentGoal.NextGoal, this.TargetFaction, out Goal newGoal);
                this.CurrentGoal = newGoal;
            }
        }
    }
}

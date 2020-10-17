using BSI.Core.Enumerations;
using BSI.Core.Extensions;
using BSI.Core.Helpers;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger
{
    public class GameManager : CampaignBehaviorBase
    {
        private List<Trigger> Triggers { get; set; }

        private Dictionary<MBObjectBase, PlotManager> GamePlots { get; set; }

        private PlotManager GlobalPlots { get; set; }

        public GameManager()
        {
            this.Triggers = new List<Trigger>();
            this.GamePlots = new Dictionary<MBObjectBase, PlotManager>();
        }
        public void ClosePlot(Plot plot)
        {
            this.GamePlots[plot.Parent].RemovePlot(plot);
        }
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, (kingdom) => this.GamePlots.Add(kingdom, new PlotManager()));
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, (kingdom) => this.GamePlots.Remove(kingdom));
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, (clan) => this.GamePlots.Remove(clan));
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        public void InitializeTrigger()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(fileInfo.Directory.Parent.Parent.FullName, "Plots");
            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    var type = assembly.ExportedTypes.FirstOrDefault(t => t.IsSubclassOf(typeof(Trigger)));
                    if(type == null)
                    {
                        continue;
                    }

                    var trigger = (Trigger)Activator.CreateInstance(type);
                    this.Triggers.Add(trigger);
                }
                catch (Exception)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Cloak and Dagger: Failed to initialize plot!", ColorHelper.Red));
                }
            }
        }

        public void InitializeGamePlots()
        {
            foreach (var kingdom in Campaign.Current.Kingdoms)
            {
                this.GamePlots.Add(kingdom, new PlotManager());
            }

            foreach (var clan in Campaign.Current.Clans)
            {
                this.GamePlots.Add(clan, new PlotManager());
            }

            foreach (var character in Campaign.Current.Characters)
            {
                this.GamePlots.Add(character, new PlotManager());
            }
        }

        private void OnDailyTick()
        {
            foreach (var trigger in this.Triggers)
            {
                var relevantGameObjects = new List<MBObjectBase>();

                //Check game objects with an active plot for current trigger
                switch (trigger.UniqueTo)
                {
                    case UniqueTo.NotSet:
                        break;
                    case UniqueTo.Global:
                        break;
                    case UniqueTo.Character:
                        foreach (var gameObject in this.GamePlots.Keys.Where(gameObject => gameObject.GetType() == typeof(CharacterObject)))
                        {
                            var character = (CharacterObject)gameObject;
                            if (trigger.CanStart(character))
                            {
                                var plot = trigger.Start(character);
                                this.GamePlots[character].AddPlot(plot);
                                relevantGameObjects.Add(character);
                            }
                        }
                        break;
                    case UniqueTo.Clan:
                        foreach (var gameObject in this.GamePlots.Keys.Where(gameObject => gameObject.GetType() == typeof(Clan)))
                        {
                            var clan = (Kingdom)gameObject;
                            if (trigger.CanStart(clan))
                            {
                                var plot = trigger.Start(clan);
                                this.GamePlots[clan].AddPlot(plot);
                                relevantGameObjects.Add(clan);
                            }
                        }
                        break;
                    case UniqueTo.Kingdom:
                        foreach (var gameObject in this.GamePlots.Keys.Where(gameObject => gameObject.GetType() == typeof(Kingdom)))
                        {
                            var kingdom = (Kingdom)gameObject;
                            if (trigger.CanStart(kingdom))
                            {
                                var plot = trigger.Start(kingdom);
                                this.GamePlots[kingdom].AddPlot(plot);
                                relevantGameObjects.Add(kingdom);
                            }
                        }
                        break;
                }

                foreach (var gameObject in relevantGameObjects)
                {
                    foreach (var plot in this.GamePlots[gameObject].Plots.Where(plot => plot.TriggerType == trigger.GetType()))
                    {
                        plot.CurrentGoal.Behavior.OnDailyTick(plot);
                    }
                }
            }
        }
    }
}

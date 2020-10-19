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

        private PlotManager GlobalPlots { get; set; }

        private Dictionary<MBObjectBase, PlotManager> GamePlots { get; set; }

        public GameManager()
        {
            this.Triggers = new List<Trigger>();
            this.GlobalPlots = new PlotManager();
            this.GamePlots = new Dictionary<MBObjectBase, PlotManager>();
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

        public void InitializeTriggers()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(fileInfo.Directory.Parent.Parent.FullName, "Plots");
            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    var type = assembly.ExportedTypes.FirstOrDefault(t => t.IsSubclassOf(typeof(Trigger)));
                    if (type == null)
                    {
                        continue;
                    }

                    var trigger = (Trigger)Activator.CreateInstance(type);
                    this.Triggers.Add(trigger);
                }
                catch (Exception)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Cloak and Dagger: Failed to initialize plot!", ColorHelper.Colors.Red));
                }
            }
        }

        public void InitializeGameObjects()
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

                foreach (var gameObject in this.GamePlots.Keys)
                {
                    var existingPlotsCount = this.GamePlots[gameObject].Plots.Count(plot => plot.TriggerType == trigger.GetType());

                    if (existingPlotsCount > 0)
                    {
                        relevantGameObjects.Add(gameObject);
                    }

                    if (trigger.AllowedInstancesPerGameObject > existingPlotsCount && trigger.CanStart(gameObject))
                    {
                        var plot = trigger.Start(gameObject);
                        this.GamePlots[gameObject].AddPlot(plot);
                        relevantGameObjects.Add(gameObject);
                    }
                }

                foreach (var gameObject in relevantGameObjects)
                {
                    foreach (var plot in this.GamePlots[gameObject].Plots.Where(plot => plot.TriggerType == trigger.GetType()))
                    {
                        var behavior = plot.CurrentGoal.Behavior;

                        behavior.OnDailyTick(plot);
                        if (behavior.CanEnd(plot))
                        {
                            var isEndGoal = plot.IsEndGoal();
                            behavior.DoEnd(plot);

                            if (isEndGoal)
                            {
                                this.GamePlots[gameObject].RemovePlot(plot);
                            }
                        }
                    }
                }
            }
        }
    }
}

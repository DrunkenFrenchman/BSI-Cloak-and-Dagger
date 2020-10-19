using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDager.Managers
{
    public class GameManager : CampaignBehaviorBase
    {
        private List<Trigger> Triggers { get; set; }

        private PlotManager GlobalPlots { get; set; }

        private Dictionary<MBObjectBase, PlotManager> GamePlots { get; set; }

        public GameManager()
        {
            Triggers = new List<Trigger>();
            GlobalPlots = new PlotManager();
            GamePlots = new Dictionary<MBObjectBase, PlotManager>();
        }

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(OnDailyTick));
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, (kingdom) => GamePlots.Add(kingdom, new PlotManager()));
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, (kingdom) => GamePlots.Remove(kingdom));
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, (clan) => GamePlots.Remove(clan));
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        public void InitializeTriggers()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var path = fileInfo.Directory.Parent.Parent.Parent.FullName;
            foreach (var file in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
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
                    Triggers.Add(trigger);
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
                GamePlots.Add(kingdom, new PlotManager());
            }

            foreach (var clan in Campaign.Current.Clans)
            {
                GamePlots.Add(clan, new PlotManager());
            }

            foreach (var character in Campaign.Current.Characters)
            {
                GamePlots.Add(character, new PlotManager());
            }
        }

        private void OnDailyTick()
        {
            foreach (var trigger in Triggers)
            {
                var relevantGameObjects = new List<MBObjectBase>();

                foreach (var gameObject in GamePlots.Keys)
                {
                    var existingPlotsCount = GamePlots[gameObject].Plots.Count(plot => plot.TriggerType == trigger.GetType());

                    if (existingPlotsCount > 0)
                    {
                        relevantGameObjects.Add(gameObject);
                    }

                    if (trigger.AllowedInstancesPerGameObject > existingPlotsCount && trigger.CanStart(gameObject))
                    {
                        var plot = trigger.Start(gameObject);
                        GamePlots[gameObject].AddPlot(plot);
                        relevantGameObjects.Add(gameObject);
                    }
                }

                foreach (var gameObject in relevantGameObjects)
                {
                    foreach (var plot in GamePlots[gameObject].Plots.Where(plot => plot.TriggerType == trigger.GetType()))
                    {
                        var behavior = plot.CurrentGoal.Behavior;

                        if (behavior.CanEnd())
                        {
                            var isEndGoal = plot.IsEndGoalReached();
                            behavior.DoEnd();

                            if (isEndGoal)
                            {
                                GamePlots[gameObject].RemovePlot(plot);
                            }
                        }
                    }
                }
            }
        }
    }
}

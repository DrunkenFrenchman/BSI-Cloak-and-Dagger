using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Managers
{
    public class GameManager : CampaignBehaviorBase
    {
        #region Thread-Safe Singleton

        private static volatile GameManager instance;
        private static readonly object syncRoot = new object();

        private GameManager()
        {
            Triggers = new List<Trigger>();
            PlotManager = new Dictionary<string, List<Plot>>();
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GameManager();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        private List<Trigger> Triggers { get; set; }

        public Dictionary<string, List<Plot>> PlotManager { get; set; }

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(OnDailyTick));
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, (kingdom) =>
            {
                PlotManager.Add(kingdom.StringId, new List<Plot>());
            });
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, (kingdom) =>
            {
                PlotManager.Remove(kingdom.StringId);
                KingdomHelper.RemoveKingdom(kingdom);
            });
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, (clan) =>
            {
                PlotManager.Remove(clan.StringId);
            });
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        public bool AddTrigger(Trigger trigger)
        {
            if (Triggers.Any(t => t.GetType() == trigger.GetType()))
            {
                return false;
            }

            Triggers.Add(trigger);
            return true;
        }

        public void Initialize()
        {
            SaveFileManager.LoadData();
            foreach (var plot in PlotManager.SelectMany(p => p.Value))
            {
                plot.Initialize(plot.Title, plot.Description, plot.Target, plot.Leader, plot.MemberIds, plot.ActiveGoal, plot.EndGoal, plot.TriggerTypeName);

                var activeGoal = plot.ActiveGoal;
                plot.ActiveGoal.Initialize(activeGoal.Title, activeGoal.Description, activeGoal.NextGoal, plot);

                var endGoal = plot.EndGoal;
                plot.EndGoal.Initialize(endGoal.Title, endGoal.Description, endGoal.NextGoal, plot);
            }

            var stringIds = new List<string>();
            stringIds.AddRange(Campaign.Current.Kingdoms.Select(k => k.StringId));
            stringIds.AddRange(Campaign.Current.Clans.Select(c => c.StringId));
            stringIds.AddRange(Campaign.Current.Heroes.Select(h => h.StringId));
            stringIds.AddRange(Campaign.Current.Characters.Select(c => c.StringId));

            foreach (var stringId in stringIds.Except(PlotManager.Keys))
            {
                PlotManager.Add(stringId, new List<Plot>());
            }
        }

        private void OnDailyTick()
        {
            foreach (var trigger in Triggers)
            {
                var relevantGameObjects = new List<MBObjectBase>();

                foreach (var gameObject in GameObjectHelper.GetGameObjectsByStringIds(PlotManager.Keys.ToList()))
                {
                    var existingPlotsCount = PlotManager[gameObject.StringId].Count(plot => plot.TriggerTypeName == trigger.GetType().Name);

                    if (existingPlotsCount > 0)
                    {
                        relevantGameObjects.Add(gameObject);
                    }

                    if(existingPlotsCount == trigger.AllowedInstances)
                    {
                        continue;
                    }

                    if (!relevantGameObjects.Contains(gameObject) && trigger.CanStart(gameObject))
                    {
                        var plot = trigger.DoStart(gameObject);

                        PlotManager[gameObject.StringId].Add(plot);
                        relevantGameObjects.Add(gameObject);
                    }
                }

                foreach (var gameObject in relevantGameObjects)
                {
                    var plotsToRemove = new List<Plot>();

                    foreach (var plot in PlotManager[gameObject.StringId].Where(p => p.TriggerTypeName == trigger.GetType().Name))
                    {
                        if (plot.CanAbort())
                        {
                            plot.DoAbort();
                            plotsToRemove.Add(plot);
                        }
                        else
                        {
                            var behavior = plot.ActiveGoal.Behavior;
                            if (behavior.CanEnd())
                            {
                                behavior.DoEnd();

                                if (plot.IsEndGoal())
                                {
                                    plotsToRemove.Add(plot);
                                }
                                else
                                {
                                    plot.SetNextGoal();
                                }
                            }
                        }
                    }

                    foreach (var plot in plotsToRemove)
                    {
                        PlotManager[gameObject.StringId].Remove(plot);
                    }
                }
            }
        }
    }
}
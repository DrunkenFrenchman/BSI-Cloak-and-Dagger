using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.Managers
{
    public class GameManager : CampaignBehaviorBase
    {
        #region Thread-Safe Singleton

        private static volatile GameManager _instance;
        private static readonly object SyncRoot = new object();

        private GameManager()
        {
            Triggers = new List<Trigger>();
            PlotManager = new Dictionary<GameObject, List<Plot>>();
        }

        public static GameManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new GameManager();
                    }
                }

                return _instance;
            }
        }

        #endregion

        public List<Trigger> Triggers { get; internal set; }

        public Dictionary<GameObject, List<Plot>> PlotManager { get; internal set; }

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, kingdom =>
            {
                PlotManager.Add(new GameObject { GameObjectType = GameObjectType.Kingdom, StringId = kingdom.StringId }, new List<Plot>());
            });
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, kingdom =>
            {
                PlotManager.Remove(PlotManager.FirstOrDefault(c => c.Key.GameObjectType == GameObjectType.Kingdom && c.Key.StringId == kingdom.StringId).Key);
                KingdomHelper.RemoveKingdom(kingdom);
            });
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, clan =>
            {
                PlotManager.Remove(PlotManager.FirstOrDefault(c => c.Key.GameObjectType == GameObjectType.Clan && c.Key.StringId == clan.StringId).Key);
            });
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        public void AddTrigger(Trigger trigger)
        {
            if (Triggers.Any(t => t.GetType() == trigger.GetType()))
            {
                return;
            }

            Triggers.Add(trigger);
        }

        private void OnDailyTick()
        {
            if (!SaveFileManager.Instance.IsFirstDailyTickDataLoaded)
            {
                SaveFileManager.Instance.LoadData();
                SaveFileManager.Instance.IsFirstDailyTickDataLoaded = true;
            }

            foreach (var trigger in Triggers)
            {
                var relevantGameObjects = new List<MBObjectBase>();

                foreach (var gameObject in GameObjectHelper.GetMBObjectsByGameObjects(PlotManager.Keys.ToList()))
                {
                    var existingPlotsCount = PlotManager.Where(p => p.Key.StringId == gameObject.StringId).SelectMany(p => p.Value).Count(p => p.TriggerType == trigger.GetType().Name);

                    if (existingPlotsCount > 0)
                    {
                        relevantGameObjects.Add(gameObject);
                    }

                    if (existingPlotsCount == trigger.AllowedInstances)
                    {
                        continue;
                    }

                    if (relevantGameObjects.Contains(gameObject) || !trigger.CanStart(gameObject))
                    {
                        continue;
                    }

                    var plot = trigger.DoStart(gameObject);
                    PlotManager.FirstOrDefault(p => p.Key.StringId == gameObject.StringId).Value.Add(plot);
                    relevantGameObjects.Add(gameObject);
                }

                foreach (var gameObject in relevantGameObjects)
                {
                    var plotsToRemove = new List<Plot>();

                    foreach (var plot in PlotManager.Where(p => p.Key.StringId == gameObject.StringId).SelectMany(p => p.Value))
                    {
                        if (plot.CanAbort())
                        {
                            plot.DoAbort();
                            plotsToRemove.Add(plot);
                        }
                        else
                        {
                            var behavior = plot.ActiveGoal.Behavior;
                            if (!behavior.CanEnd())
                            {
                                continue;
                            }

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

                    foreach (var plot in plotsToRemove)
                    {
                        PlotManager.FirstOrDefault(c => c.Key.StringId == gameObject.StringId).Value.Remove(plot);
                    }
                }
            }
        }
    }
}
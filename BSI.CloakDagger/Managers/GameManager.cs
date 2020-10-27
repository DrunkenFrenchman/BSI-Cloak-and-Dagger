using System.Collections.Generic;
using System.Linq;
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
            PlotManager = new Dictionary<string, List<Plot>>();
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

        private List<Trigger> Triggers { get; }

        public Dictionary<string, List<Plot>> PlotManager { get; set; }

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, kingdom =>
            {
                PlotManager.Add(kingdom.StringId, new List<Plot>());
            });
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, kingdom =>
            {
                PlotManager.Remove(kingdom.StringId);
                KingdomHelper.RemoveKingdom(kingdom);
            });
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, clan =>
            {
                PlotManager.Remove(clan.StringId);
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
                    var existingPlotsCount = PlotManager[gameObject.StringId].Count(p => p.TriggerTypeName == trigger.GetType().Name);

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
                    PlotManager[gameObject.StringId].Add(plot);
                    relevantGameObjects.Add(gameObject);
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
                        PlotManager[gameObject.StringId].Remove(plot);
                    }
                }
            }
        }
    }
}
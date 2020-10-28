using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.Managers
{
    public class GameManager : CampaignBehaviorBase
    {
        private bool _isFirstDailyTick = true;

        public List<Trigger> Triggers { get; internal set; }

        public List<GameObject> GameObjects { get; internal set; }

        public PlotManager PlotManager { get; internal set; }

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);

            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, kingdom =>
            {
                GameObjects.Add(new GameObject
                {
                    GameObjectType = GameObjectType.Kingdom,
                    StringId = kingdom.StringId
                });
            });

            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, kingdom =>
            {
                GameObjects.RemoveAll(g => g.GameObjectType == GameObjectType.Kingdom && g.StringId == kingdom.StringId);
                KingdomHelper.RemoveKingdom(kingdom);
            });

            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, clan =>
            {
                GameObjects.RemoveAll(g => g.GameObjectType == GameObjectType.Clan && g.StringId == clan.StringId);
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

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            LoadGameObjects();
        }

        private void OnDailyTick()
        {
            if (_isFirstDailyTick)
            {
                LoadGameObjects();
                _isFirstDailyTick = false;
            }

            var relevantGameObjects = new List<GameObject>();

            foreach (var trigger in Triggers)
            {
                foreach (var gameObject in GameObjects)
                {
                    var existingPlotsCount = PlotManager.GamePlots[gameObject].Count(p => p.TriggerType == trigger.GetType().Name);

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

                    PlotManager.Add(gameObject, trigger.DoStart(gameObject));
                    relevantGameObjects.Add(gameObject);
                }
            }

            foreach (var plot in relevantGameObjects.SelectMany(gameObject => PlotManager.GamePlots[gameObject]))
            {
                if (plot.CanAbort())
                {
                    plot.DoAbort();
                    PlotManager.Remove(plot);
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
                        PlotManager.Remove(plot);
                    }
                    else
                    {
                        plot.SetNextGoal();
                    }
                }
            }
        }

        private void LoadGameObjects()
        {
            var kingdoms = Campaign.Current.Kingdoms.Select(k => new GameObject
            {
                GameObjectType = GameObjectType.Kingdom,
                StringId = k.StringId
            });
            foreach (var kingdom in kingdoms.Except(Instance.GameObjects))
            {
                Instance.GameObjects.Add(kingdom);
            }

            var clans = Campaign.Current.Clans.Select(c => new GameObject
            {
                GameObjectType = GameObjectType.Clan,
                StringId = c.StringId
            });
            foreach (var clan in clans.Except(Instance.GameObjects))
            {
                Instance.GameObjects.Add(clan);
            }

            var heroes = Campaign.Current.Heroes.Select(h => new GameObject
            {
                GameObjectType = GameObjectType.Hero,
                StringId = h.StringId
            });
            foreach (var hero in heroes.Except(Instance.GameObjects))
            {
                Instance.GameObjects.Add(hero);
            }

            var characters = Campaign.Current.Characters.Select(c => new GameObject
            {
                GameObjectType = GameObjectType.Character,
                StringId = c.StringId
            });
            foreach (var character in characters.Except(Instance.GameObjects))
            {
                Instance.GameObjects.Add(character);
            }
        }

        #region Thread-Safe Singleton

        private static volatile GameManager _instance;
        private static readonly object SyncRoot = new object();

        private GameManager()
        {
            Triggers = new List<Trigger>();
            GameObjects = new List<GameObject>();
            PlotManager = PlotManager.Instance;
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
    }
}
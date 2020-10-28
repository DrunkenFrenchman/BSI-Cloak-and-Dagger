using System.Collections.Generic;
using System.Linq;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Extensions;
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

            foreach (var trigger in Triggers)
            {
                foreach (var gameObject in GameObjects.Where(go => go.GameObjectType == trigger.InitiatorType))
                {
                    if (!trigger.CanStart(gameObject))
                    {
                        continue;
                    }

                    PlotManager.Add(trigger.DoStart(gameObject));
                }
            }

            foreach (var plot in PlotManager.GetPlots())
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
            GameObjects.AddRange(Campaign.Current.Kingdoms.ToGameObjects().Except(GameObjects));
            GameObjects.AddRange(Campaign.Current.Clans.ToGameObjects().Except(GameObjects));
            GameObjects.AddRange(Campaign.Current.Heroes.ToGameObjects().Except(GameObjects));
            GameObjects.AddRange(Campaign.Current.Characters.ToGameObjects().Except(GameObjects));
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
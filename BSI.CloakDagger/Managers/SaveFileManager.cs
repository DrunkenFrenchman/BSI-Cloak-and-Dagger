using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bannerlord.ButterLib.Common.Extensions;
using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using Newtonsoft.Json;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace BSI.CloakDagger.Managers
{
    internal class SaveFileManager
    {
        #region Thread-Safe Singleton

        private static volatile SaveFileManager _instance;
        private static readonly object SyncRoot = new object();

        private SaveFileManager()
        {

        }

        public static SaveFileManager Instance
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
                        _instance = new SaveFileManager();
                    }
                }

                return _instance;
            }
        }

        #endregion

        public string ActiveSaveSlotName { get; internal set; }

        public bool IsSaveDataLoaded { get; private set; }

        public bool IsFirstDailyTickDataLoaded { get; internal set; }

        public string SavePath => Path.Combine(SaveFolderPath, SaveFileName);

        public string SaveFolderPath => Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Saves");

        public string SaveFileName => $"{ActiveSaveSlotName}.json";

        public void SaveData()
        {
            Directory.CreateDirectory(SaveFolderPath);
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(GameManager.Instance.PlotManager,new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public void LoadData()
        {
            if (File.Exists(SavePath) && !IsSaveDataLoaded)
            {
                foreach (var (key, value) in JsonConvert.DeserializeObject<Dictionary<GameObject, List<Plot>>>(File.ReadAllText(SavePath)))
                {
                    GameManager.Instance.PlotManager.Add(key, value);
                }

                IsSaveDataLoaded = true;
            }

            var kingdoms = Campaign.Current.Kingdoms.Select(k => new GameObject {GameObjectType = GameObjectType.Kingdom, StringId = k.StringId});
            foreach (var kingdom in kingdoms.Except(GameManager.Instance.PlotManager.Keys))
            {
                GameManager.Instance.PlotManager.Add(kingdom, new List<Plot>());
            }

            var clans = Campaign.Current.Clans.Select(c => new GameObject { GameObjectType = GameObjectType.Clan, StringId = c.StringId });
            foreach (var clan in clans.Except(GameManager.Instance.PlotManager.Keys))
            {
                GameManager.Instance.PlotManager.Add(clan, new List<Plot>());
            }

            var heroes = Campaign.Current.Heroes.Select(h => new GameObject { GameObjectType = GameObjectType.Hero, StringId = h.StringId });
            foreach (var hero in heroes.Except(GameManager.Instance.PlotManager.Keys))
            {
                GameManager.Instance.PlotManager.Add(hero, new List<Plot>());
            }

            var characters = Campaign.Current.Characters.Select(c => new GameObject { GameObjectType = GameObjectType.Character, StringId = c.StringId });
            foreach (var character in characters.Except(GameManager.Instance.PlotManager.Keys))
            {
                GameManager.Instance.PlotManager.Add(character, new List<Plot>());
            }

            foreach (var plot in GameManager.Instance.PlotManager.SelectMany(p => p.Value))
            {
                plot.Initialize(plot.Title, plot.Description, plot.Target, plot.Leader, plot.Members, plot.ActiveGoal, plot.EndGoal, plot.TriggerType);

                var activeGoal = plot.ActiveGoal;
                plot.ActiveGoal.Initialize(activeGoal.Title, activeGoal.Description, activeGoal.NextGoal, plot);

                var endGoal = plot.EndGoal;
                plot.EndGoal.Initialize(endGoal.Title, endGoal.Description, endGoal.NextGoal, plot);
            }
        }
    }
}
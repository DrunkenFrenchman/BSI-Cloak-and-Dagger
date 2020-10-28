using System.Collections.Generic;
using System.IO;
using BSI.CloakDagger.Objects;
using Newtonsoft.Json;
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

        public string SavePath => Path.Combine(SaveFolderPath, SaveFileName);

        public string SaveFolderPath => Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Saves");

        public string SaveFileName => $"{ActiveSaveSlotName}.json";

        public void SaveData()
        {
            Directory.CreateDirectory(SaveFolderPath);
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(GameManager.Instance.PlotManager.Plots, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public void LoadData()
        {
            if (!File.Exists(SavePath))
            {
                return;
            }

            GameManager.Instance.PlotManager.Plots = JsonConvert.DeserializeObject<List<GamePlot>>(File.ReadAllText(SavePath));

            foreach (var plot in GameManager.Instance.PlotManager.GetPlots())
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
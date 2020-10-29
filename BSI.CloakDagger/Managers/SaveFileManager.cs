using System.Collections.Generic;
using System.IO;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Models;
using Newtonsoft.Json;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace BSI.CloakDagger.Managers
{
    internal class SaveFileManager
    {
        public string ActiveSaveSlotName { get; internal set; }

        public string SavePath => Path.Combine(SaveFolderPath, SaveFileName);

        public string SaveFolderPath => Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Saves");

        public string SaveFileName => $"{ActiveSaveSlotName}.json";

        public void Save()
        {
            Directory.CreateDirectory(SaveFolderPath);
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(GameManager.Instance.PlotManager.Plots, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }

        public void Load()
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

        public void Delete()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }

            if (File.Exists(LogHelper.FilePath))
            {
                File.Delete(LogHelper.FilePath);
            }
        }

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
    }
}
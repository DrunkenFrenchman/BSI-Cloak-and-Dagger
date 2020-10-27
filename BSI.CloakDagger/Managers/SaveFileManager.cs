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

        public string SaveFilePath => Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Saves", $"{ActiveSaveSlotName}.json");

        public void SaveData()
        {
            Directory.CreateDirectory(SaveFilePath);
            File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(GameManager.Instance.PlotManager,new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public void LoadData()
        {
            if (!File.Exists(SaveFilePath))
            {
                return;
            }

            GameManager.Instance.PlotManager = JsonConvert.DeserializeObject<Dictionary<string, List<Plot>>>(File.ReadAllText(SaveFilePath));
        }
    }
}
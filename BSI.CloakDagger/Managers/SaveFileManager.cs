using BSI.CloakDagger.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace BSI.CloakDagger.Managers
{
    internal class SaveFileManager
    {
        internal static string ActiveSaveSlotName { get; set; }

        internal static string SaveFilePath => Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Saves", $"{ActiveSaveSlotName}.json");

        internal static void SaveData()
        {
            Directory.CreateDirectory(SaveFilePath);
            File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(GameManager.Instance.Plots, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        internal static void LoadData()
        {
            if(!File.Exists(SaveFilePath))
            {
                return;
            }

            GameManager.Instance.Plots = JsonConvert.DeserializeObject<Dictionary<string, List<Plot>>>(File.ReadAllText(SaveFilePath));
        }
    }
}
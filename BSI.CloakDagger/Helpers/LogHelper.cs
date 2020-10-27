using System;
using System.IO;
using BSI.CloakDagger.Managers;
using BSI.CloakDagger.Settings;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace BSI.CloakDagger.Helpers
{
    public static class LogHelper
    {
        private static readonly CloakDaggerSettings Settings = CloakDaggerSettings.Instance;

        private static readonly string FolderPath = Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Logs");
        private static readonly string FileName = $"{(string.IsNullOrEmpty(SaveFileManager.Instance.ActiveSaveSlotName) ? "ERROR" : SaveFileManager.Instance.ActiveSaveSlotName)}.log";

        public static void Log(string message)
        {
            if (!Settings.EnableDebug)
            {
                return;
            }

            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    Initialize();
                }
                
                File.AppendAllText(Path.Combine(FolderPath, FileName), $"{DateTime.Now} | {message}{Environment.NewLine}");
            }
            catch (Exception exception)
            {
                LogException("ERROR", exception);
            }
        }

        public static void LogException(string name, Exception exception)
        {
            Log(name + ": " + exception.Message);
            Log(exception.StackTrace);
        }

        public static void DisplayMessage(string message, bool appendToLog = true)
        {
            if (!Settings.EnableDebug)
            {
                return;
            }

            InformationManager.DisplayMessage(message != null ? new InformationMessage(message) : new InformationMessage(typeof(LogHelper).Namespace + " tried printing null message!"));

            if (appendToLog)
            {
                Log(message);
            }
        }

        private static void Initialize()
        {
            Directory.CreateDirectory(FolderPath);
            Log("New Session Started");
        }
    }
}
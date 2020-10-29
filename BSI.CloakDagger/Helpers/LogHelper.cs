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
        internal static readonly string FolderPath = Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Logs");
        internal static readonly string FileName = $"{(string.IsNullOrEmpty(SaveFileManager.Instance.ActiveSaveSlotName) ? "ERROR" : SaveFileManager.Instance.ActiveSaveSlotName)}.log";

        internal static string FilePath => Path.Combine(FolderPath, FileName);

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

                File.AppendAllText(FilePath, $"{DateTime.Now} | {message}{Environment.NewLine}");
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
using System;
using System.Globalization;
using System.IO;
using BSI.CloakDagger.Managers;
using BSI.CloakDagger.Settings;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using Path = System.IO.Path;

namespace BSI.CloakDagger
{
    public class Debug
    {
        private static readonly CloakDaggerSettings Settings = CloakDaggerSettings.Instance;

        private static readonly string FileName = $"{(string.IsNullOrEmpty(SaveFileManager.ActiveSaveSlotName) ? "ERROR" : SaveFileManager.ActiveSaveSlotName)}.log";

        //Print Message in Game Helper
        public static void PrintMessage(string message)
        {
            if (!Settings.EnableDebug)
            {
                return;
            }

            InformationManager.DisplayMessage(message != null ? new InformationMessage(message) : new InformationMessage(typeof(Debug).Namespace + " tried printing null message!"));
        }

        //Return Date and Time helper
        public static string DateTime()
        {
            return System.DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }

        //Log File Direectory Helper
        public static string GetDirectory()
        {
            return Path.Combine(Utilities.GetConfigsPath(), "CloakDagger", "Logs");
        }

        //Add Log Helper
        public static void AddEntry(string entry)
        {
            if (!Settings.EnableDebug)
            {
                return;
            }

            //Create Directory
            var path = GetDirectory();
            if (!Directory.Exists(path))
            {
                DebugStart();
            }

            try
            {
                if (entry == "New Session Start: Module Loaded")
                {
                    entry = "\n\n\n" + DateTime() + "==>" + entry;
                }
                else
                {
                    entry = "\n" + DateTime() + "==>" + entry;
                }
            }
            catch (Exception ex)
            {
                AddExceptionLog("ERROR", ex);
            }

            File.AppendAllText(Path.Combine(GetDirectory(), FileName), entry);
        }

        //Exception Log Helper
        public static void AddExceptionLog(string name, Exception ex)
        {
            AddEntry(name + ": " + ex.Message);
            AddEntry(ex.StackTrace);
        }

        //Initialize Debug
        public static void DebugStart()
        {
            if (!Directory.Exists(GetDirectory()))
            {
                Directory.CreateDirectory(GetDirectory());
                PrintMessage(typeof(Debug).Namespace + " Debug File Path Created");
                AddEntry("Log Folder Created");
            }

            AddEntry("New Session Start: Module Loaded");
        }
    }
}
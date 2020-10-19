﻿using System;
using System.IO;
using TaleWorlds.Core;

namespace BSI.CloakDagger
{
    public class Debug
    {
        private static readonly CloakDaggerSettings settings = CloakDaggerSettings.Instance;
        private static readonly string fileName = typeof(Debug).Namespace + ".debug.log";

        //Print Message in Game Helper
        public static void PrintMessage(string message)
        {
            if (settings.BSICORE_DEBUG)
            {
                if (message != null)
                {
                    InformationManager.DisplayMessage(new InformationMessage(message));
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage(typeof(Debug).Namespace + " tried printing null message!"));
                }
            }
        }

        //Return Date and Time helper
        public static string DateTime()
        {
            return System.DateTime.Now.ToString();
        }

        //Log File Direectory Helper
        public static string GetDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Mount and Blade II Bannerlord", "Logs");
        }

        //Add Log Helper
        public static void AddEntry(string entry)
        {
            if (!settings.BSICORE_DEBUG)
            {
                return;
            }

            //Create Directory
            string path = GetDirectory();
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

            File.AppendAllText(Path.Combine(GetDirectory(), fileName), entry);
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
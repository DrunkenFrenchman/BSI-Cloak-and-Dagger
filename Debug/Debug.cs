using System;
using System.IO;
using TaleWorlds.Core;

namespace BSI.CivilWars
{

    public class Debug
    {
        private static readonly BSI.CivilWars.Settings settings = BSI.CivilWars.Settings.Instance;
        private static readonly string fileName = typeof(Debug).Namespace + ".debug.log";

        //Print Message in Game Helper
        public static void PrintMessage(string message)
        {
            if (settings.BSICWDebug is true)
            {
                if (message != null) { InformationManager.DisplayMessage(new InformationMessage(message)); }
                else { InformationManager.DisplayMessage(new InformationMessage(typeof(Debug).Namespace + " tried printing null message!")); }
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
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = System.IO.Path.Combine(filePath, "Mount and Blade II Bannerlord");
            filePath = System.IO.Path.Combine(filePath, "Logs");
            return filePath;
        }

        //Add Log Helper
        public static void AddEntry(string entry)
        {
            if (settings.BSICWDebug is false) { return; }

            //Create Directory
            string path = GetDirectory();
            if (System.IO.Directory.Exists(path) != true)
            {
                DebugStart();
            }
            try
            {
                if (entry == "New Session Start: Module Loaded") { entry = "\n\n\n" + DateTime() + "==>" + entry; }
                else { entry = "\n" + DateTime() + "==>" + entry; }
            }
            catch (Exception ex) { AddExceptionLog("ERROR", ex); }

            System.IO.File.AppendAllText(Path.Combine(GetDirectory(), fileName), entry);

        }
        //Exception Log Helper
        public static void AddExceptionLog(string name, Exception ex)
        {
            Debug.AddEntry(name + ": " + ex.Message);
            Debug.AddEntry(ex.StackTrace);
        }

        //Initialize Debug
        public static void DebugStart()
        {

            if (!System.IO.Directory.Exists(GetDirectory()))
            {
                System.IO.Directory.CreateDirectory(GetDirectory());
                Debug.PrintMessage(typeof(Debug).Namespace + " Debug File Path Created");
                AddEntry("Log Folder Created");
            }

            AddEntry("New Session Start: Module Loaded");
        }
    }
}

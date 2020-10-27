using System;
using BSI.CloakDagger.Helpers;
using BSI.CloakDagger.Managers;
using HarmonyLib;
using TaleWorlds.Core;

namespace BSI.CloakDagger.Patches
{
    internal static class SaveFilePatches
    {
        [HarmonyPatch(typeof(MBSaveLoad), "OnNewGame")]
        internal static class OnNewGame
        {
            internal static void Postfix()
            {
                try
                {
                    SaveFileManager.ActiveSaveSlotName = string.Empty;
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("OnNewGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "LoadSaveGameData")]
        internal static class LoadSaveGameData
        {
            internal static void Postfix()
            {
                try
                {
                    SaveFileManager.ActiveSaveSlotName = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName").GetValue(null).ToString();
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("LoadSaveGameData", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "SaveAsCurrentGame")]
        internal static class SaveAsCurrentGame
        {
            internal static void Postfix()
            {
                try
                {
                    SaveFileManager.ActiveSaveSlotName = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName")?.GetValue(null)?.ToString();
                    SaveFileManager.SaveData();
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("SaveAsCurrentGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "QuickSaveCurrentGame")]
        internal static class QuickSaveCurrentGame
        {
            internal static void Postfix()
            {
                try
                {
                    SaveFileManager.ActiveSaveSlotName = AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName").GetValue(null).ToString();
                    SaveFileManager.SaveData();
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("QuickSaveCurrentGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "AutoSaveCurrentGame")]
        internal static class AutoSaveCurrentGame
        {
            internal static void Postfix()
            {
                try
                {
                    SaveFileManager.ActiveSaveSlotName = $"{AccessTools.Field(typeof(MBSaveLoad), "AutoSaveNamePrefix").GetValue(null)}{AccessTools.Field(typeof(MBSaveLoad), "AutoSaveIndex").GetValue(null)}";
                    SaveFileManager.SaveData();
                }
                catch (Exception exception)
                {
                    Debug.AddExceptionLog("AutoSaveCurrentGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }
    }
}
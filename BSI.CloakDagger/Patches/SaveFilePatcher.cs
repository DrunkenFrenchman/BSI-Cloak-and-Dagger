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
                    SaveFileManager.Instance.ActiveSaveSlotName = string.Empty;
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("OnNewGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "LoadSaveGameData")]
        internal static class LoadSaveGameData
        {
            internal static void Postfix(string saveName)
            {
                try
                {
                    SaveFileManager.Instance.ActiveSaveSlotName = saveName;
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("LoadSaveGameData", exception);
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
                    SaveFileManager.Instance.ActiveSaveSlotName = $"{AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName").GetValue(null)}";
                    SaveFileManager.Instance.Save();
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("SaveAsCurrentGame", exception);
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
                    SaveFileManager.Instance.ActiveSaveSlotName = $"{AccessTools.Field(typeof(MBSaveLoad), "ActiveSaveSlotName").GetValue(null)}";
                    SaveFileManager.Instance.Save();
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("QuickSaveCurrentGame", exception);
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
                    SaveFileManager.Instance.ActiveSaveSlotName = $"{AccessTools.Field(typeof(MBSaveLoad), "AutoSaveNamePrefix").GetValue(null)}{AccessTools.Field(typeof(MBSaveLoad), "AutoSaveIndex").GetValue(null)}";
                    SaveFileManager.Instance.Save();
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("AutoSaveCurrentGame", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }

        [HarmonyPatch(typeof(MBSaveLoad), "DeleteSaveGame")]
        internal static class DeleteSaveGame
        {
            internal static void Postfix(string saveName)
            {
                try
                {
                    SaveFileManager.Instance.ActiveSaveSlotName = saveName;
                    SaveFileManager.Instance.Delete();
                }
                catch (Exception exception)
                {
                    LogHelper.LogException("LoadSaveGameData", exception);
                    InformationManager.DisplayMessage(new InformationMessage("Cloak and Dagger", ColorHelper.Colors.Red));
                }
            }
        }
    }
}
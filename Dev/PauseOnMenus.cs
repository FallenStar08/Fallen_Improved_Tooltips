//using Fallen_LE_Mods.Shared;
//using HarmonyLib;
//using Il2Cpp;
//using MelonLoader;
//using UnityEngine;

//TODO try implementing the vanilla pausing logic happening when you open the menu

//namespace Fallen_LE_Mods.Dev
//{
//    public class UIBasedPause : MelonMod
//    {
//        private static int pauseCount = 0;

//        private static void UpdatePauseState()
//        {
//            Time.timeScale = pauseCount > 0 ? 0f : 1f;
//        }

//        private static void AddPauseSource()
//        {
//            FallenUtils.Log("Pausing");
//            pauseCount++;
//            UpdatePauseState();
//        }

//        private static void RemovePauseSource()
//        {
//            FallenUtils.Log("Resuming");
//            pauseCount = Mathf.Max(0, pauseCount - 1);
//            UpdatePauseState();
//        }

//        private static void SetPauseForPanel(bool isVisible)
//        {
//            if (isVisible) AddPauseSource();
//            else RemovePauseSource();
//        }



//        // --- Patch SkillsPanelManager.OnEnable/OnDisable ---
//        [HarmonyPatch(typeof(SkillsPanelManager))]
//        public static class SkillsPanelPatches
//        {
//            [HarmonyPostfix, HarmonyPatch("OnEnable")]
//            public static void OnEnable() => AddPauseSource();

//            [HarmonyPostfix, HarmonyPatch("OnDisable")]
//            public static void OnDisable() => RemovePauseSource();
//        }

//        // --- Patch PassiveTreeNavigable.OnEnable/OnDisable ---
//        [HarmonyPatch(typeof(PassiveTreeNavigable))]
//        public static class PassivesPanelPatches
//        {
//            PauseWhileActiveIfOffline
//            [HarmonyPostfix, HarmonyPatch("OnEnable")]
//            public static void OnEnable() => AddPauseSource();

//            [HarmonyPostfix, HarmonyPatch("OnDisable")]
//            public static void OnDisable() => RemovePauseSource();
//        }
//    }
//}

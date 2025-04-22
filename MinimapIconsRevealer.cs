using HarmonyLib;
using Il2CppDMM;
using Il2CppLE.UI;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods
{
    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class MinimapIconsRevealer : MelonMod
    {
        //private static GameObject latestObjective;
        public static void Postfix(ref LoadingScreen __instance)
        {
            //MelonLogger.Msg("Disabled a loading screen");
            // Find all GameObjects with the name "Shrine_ShowMinimapIconTrigger"
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

            //GameObject CustomObjective = CustomQuestManager.CreateQuestObjective();
            //latestObjective = CustomObjective;
            //TextMeshProUGUI CustomObjectiveText = CustomObjective.GetComponent<TextMeshProUGUI>();

            int shrineFoundCount = 0;
            Dictionary<string, int> mapInteractables = new();

            foreach (GameObject obj in allGameObjects)
            {
                //Shrines
                if (obj.name == "Shrine_ShowMinimapIconTrigger")
                {

                    string shrineName = obj.transform.parent?.transform?.parent?.transform?.parent?.gameObject?.name ?? "DefaultName";
                    shrineName = shrineName.Replace("(Clone)", "");
                    // Check if there's a child GameObject named "shrineMinimapIcon"
                    GameObject? shrineMinimapIcon = obj.transform.Find("shrineMinimapIcon")?.gameObject;
                    if (shrineMinimapIcon != null && !shrineMinimapIcon.activeSelf)
                    {
                        shrineFoundCount++;
                        DMMapIcon DMMapIconComponent = shrineMinimapIcon.GetComponent<DMMapIcon>();
                        ShrineColorManager.SetShrineColor(DMMapIconComponent, shrineName);

                        // Enable the shrineMinimapIcon GameObject
                        shrineMinimapIcon.SetActive(true);
                        Melon<MyMod>.Logger.Msg($"Activated shrineMinimapIcon for {shrineName}");
                        FallenUtils.IncrementOrInitialize(mapInteractables, shrineName);
                        //CustomObjectiveText.text += CustomObjectiveText.text + $"\n{shrineName}";
                    }
                }
                // Check if the name of the GameObject contains "Rune Prison Visuals"
                if (obj.name.Contains("Rune Prison Visuals"))
                {
                    ActivateChildMinimapIcon(obj);
                }

                // Check if the name of the GameObject contains "One Shot Cache Interactable"
                if (obj.name.Contains("One Shot Cache Interactable"))
                {
                    ActivateChildMinimapIcon(obj);
                }
            }
            if (shrineFoundCount == 0)
            {
                Melon<MyMod>.Logger.Msg($"No Shrine found in this map!");
                //GameObject.Destroy(CustomObjective);
                //GameObject.Destroy(latestObjective);
            }
            else
            {

                foreach (var kvp in mapInteractables)
                {
                    Melon<MyMod>.Logger.Msg($"Found {kvp.Value} {kvp.Key} in the current map!");
                    //CustomObjectiveText.text += CustomObjectiveText.text + $"\n{kvp.Key} x {kvp.Value}";
                }
            }

        }
        private static void ActivateChildMinimapIcon(GameObject parentObject)
        {
            // Find the MinimapIconTrigger child GameObject
            GameObject? minimapIconTrigger = parentObject.transform.Find("MinimapIconTrigger")?.gameObject;
            if (minimapIconTrigger != null)
            {
                // Find the MinimapIcon child GameObject under MinimapIconTrigger
                GameObject? minimapIcon = minimapIconTrigger.transform.Find("MinimapIcon")?.gameObject ?? minimapIconTrigger.transform.Find("Minimap Icon")?.gameObject;
                if (minimapIcon != null && !minimapIcon.activeSelf)
                {
                    // Activate the MinimapIcon child GameObject
                    minimapIcon.SetActive(true);
                    Melon<MyMod>.Logger.Msg($"Activated MinimapIcon for {parentObject.name}");
                }
            }
        }
    }
}

#if RELEASE
using Fallen_LE_Mods.MonoScripts;
using Fallen_LE_Mods.Shared;
using HarmonyLib;
using Il2CppLE.UI;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Fallen_LE_Mods.Dev
{
    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class InfoCorner : MelonMod
    {
        private static TextMeshProUGUI? text;
        private static bool initialized = false;
        private static GameObject rectangleObject;
        private static bool isVisible = true; // Flag to track visibility
        private const KeyCode ToggleKey = KeyCode.F1; // Define the key to toggle visibility (F1 for example)


        public static void Postfix(ref LoadingScreen __instance)
        {
            if (!initialized) { Init(); }
        }

        public static void ModifyCornerInfoText(string content)
        {
            if (text != null)
            {
                text.text = content;
                ResizeBackgroundToText(); // Resize background whenever text changes
            }
        }

        private static void Init()
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject questTitle = GameObject.Find("QuestName");
            var questText = questTitle.GetComponent<TextMeshProUGUI>();
            var questFont = questText.font;

            if (canvas != null)
            {
                Melon<MyMod>.Logger.Msg("Version object found!");
                // Make our bg & txt holding compo
                GameObject textObject = new GameObject("Fallen_CornerInfoText");
                textObject.layer = LayerMask.NameToLayer("UI");

                // Create a new GameObject for the black transparent rectangle
                rectangleObject = new GameObject("Fallen_BlackBackground");
                rectangleObject.layer = LayerMask.NameToLayer("UI");

                // Add Image component to the GameObject
                Image image = rectangleObject.AddComponent<Image>();

                // Set the color to black with transparency
                image.color = new Color(0, 0, 0, 0.5f); // Adjust alpha to control transparency

                // Set the RectTransform properties
                RectTransform backgroundRectTransform = rectangleObject.GetComponent<RectTransform>();
                rectangleObject.transform.SetParent(canvas.transform); // Set the canvas as the parent
                FallenUtils.SetDefaultRectTransformProperties(backgroundRectTransform);
                backgroundRectTransform.sizeDelta = new Vector2(200, 200); // Initial size, will be adjusted later

                textObject.transform.SetParent(rectangleObject.transform);
                text = textObject.AddComponent<TextMeshProUGUI>();
                Melon<MyMod>.Logger.Msg("TextMeshPro Created");
                //textObject.AddComponent<CornerUpdater>();
                text.fontSize = 16;
                if (questFont)
                {
                    text.font = questFont;
                }

                var textRectTransform = textObject.GetComponent<RectTransform>();
                FallenUtils.SetDefaultRectTransformProperties(textRectTransform);
                textObject.transform.localPosition = new Vector3(10, -30, 0);
                // Instead of attaching CornerUpdater to textObject...
                GameObject updaterObj = new GameObject("Fallen_CornerUpdater");
                updaterObj.transform.SetParent(canvas.transform);
                var updater = updaterObj.AddComponent<CornerUpdater>();
                updater.textToUpdate = text;
                updater.backgroundObject = rectangleObject;

                initialized = true;
            }
            else
            {
                Melon<MyMod>.Logger.Msg("Canvas object not found.");
            }
        }

        // Method to resize the background rectangle based on the text size
        public static void ResizeBackgroundToText()
        {
            if (text != null && rectangleObject != null)
            {
                // Calculate the preferred width and height of the text
                Vector2 textSize = text.GetPreferredValues();

                // Resize the background to match the text's size
                RectTransform backgroundRectTransform = rectangleObject.GetComponent<RectTransform>();
                backgroundRectTransform.sizeDelta = new Vector2(textSize.x + 20, textSize.y + 50); // Adding padding
            }
        }

        // Method to toggle visibility
        private static void ToggleVisibility()
        {
            if (rectangleObject != null)
            {
                isVisible = !isVisible;
                rectangleObject.SetActive(isVisible);
                text?.gameObject.SetActive(isVisible); // Toggle text visibility as well
            }
        }

        // MelonLoader Update method to check for key press
        public override void OnUpdate()
        {
            if (Input.GetKeyDown(ToggleKey))
            {
                ToggleVisibility();
            }
        }
    }
}
#endif
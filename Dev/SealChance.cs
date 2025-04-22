#if RELEASE
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods.Dev
{
    [HarmonyPatch(typeof(CraftingSlotManager), "UpdateForgingPotentialCostDisplay")]
    public class CraftingSlotManager_UpdateForgingPotentialCostDisplay : MelonMod
    {
        public static ItemData? itemData;
        public static void Postfix(ref CraftingSlotManager __instance)
        {

            OneItemContainer mainItem = __instance.GetMain();
            if (!mainItem.HasContent()) { return; }

            ItemData itemData = mainItem.getItem();
            CraftingSlotManager_UpdateForgingPotentialCostDisplay.itemData = itemData;
        }
    }

    [HarmonyPatch(typeof(AffixSlotForge), "SetVisuals")]
    public class AffixSlotForge_SetVisuals : MelonMod
    {
        public static void Postfix(ref AffixSlotForge __instance)
        {
            if (string.IsNullOrEmpty(__instance.nameTMP.text)) { return; }
            if (CraftingSlotManager_UpdateForgingPotentialCostDisplay.itemData == null) { return; }
            int tier = CraftingSlotManager_UpdateForgingPotentialCostDisplay.itemData.GetAffixTier(__instance.affixID);
            float chance = CraftingSlotManager_UpdateForgingPotentialCostDisplay.itemData.getChanceToSealAffix(tier);
            if (chance > 0)
            {
                Color gradientColor = Color.Lerp(Color.green, Color.red, 1.0f - chance);
                string htmlColor = "#" + ColorUtility.ToHtmlStringRGB(gradientColor);
                string chanceString = (chance * 100).ToString("F2");
                __instance.nameTMP.SetText(__instance.nameTMP.text + $"\r\n<size=15><color={htmlColor}>Sealing Chance : " + chanceString + "%</size></color>");
            }
            else
            {
                __instance.nameTMP.SetText(__instance.nameTMP.text + $"\r\n<size=15><color=#04aed4>Cannot be sealed</size></color>");
            }

        }
    }
}
#endif
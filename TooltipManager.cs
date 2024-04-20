using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods.Features


{

    public class TooltipManager : MelonMod
    {
        private static string originalLoreText = "";
        private static void HandleTooltipUpdate(ItemDataUnpacked item)
        {
            Rule? match = FallenUtils.MatchFilterRule(item);

            bool addedFilterText = false;
            if (match != null && (ItemList.isEquipment(item.itemType) || ItemList.isIdol(item.itemType)))
            {
                var description = match.GetRuleDescription();
                //FallenUtils.Log(match.type.ToString());
                //FallenUtils.Log(match.isEnabled.ToString());

                if (description != null)
                {
                    if (item.LoreText == "")
                    {
                        item.LoreText += $"FilterRule : {description}";
                        addedFilterText = true;
                    }
                    else
                    {
                        item.LoreText += $"\n\n</color>FilterRule : {description}";
                        addedFilterText = true;
                    }
                }
            }

            ItemDataUnpacked? matchedUniqueOrSet = FallenUtils.FindSimilarUniqueItemInStash(item);
            if (matchedUniqueOrSet != null)
            {
                int ownedLP = matchedUniqueOrSet.legendaryPotential;
                string LPdescription = (ownedLP > item.legendaryPotential) ? $"with higher LP : {ownedLP}" :
                                       (ownedLP < item.legendaryPotential) ? $"with lower LP : {ownedLP}" :
                                       matchedUniqueOrSet.Equals(item) ? "with similar LP (Self)" :
                                                                        "with similar LP (Duplicate)";
                var description = $"Already Owned " + LPdescription;

                if (item.LoreText == "")
                {
                    item.LoreText += $"{description}";
                }
                else
                {
                    item.LoreText += addedFilterText ? $"\n\n{description}" : $"\n\n</color>{description}";
                }
            }
            else if (item.isUniqueSetOrLegendary())
            {
                var description = $"Not Owned";

                if (item.LoreText == "")
                {
                    item.LoreText += $"{description}";
                }
                else
                {
                    item.LoreText += $"\n\n</color>{description}";
                }
            }

        }

        [HarmonyPatch(typeof(TooltipItemManager), "OpenTooltip", new Type[] { typeof(ItemDataUnpacked), typeof(TooltipItemManager.SlotType), typeof(Vector2), typeof(Vector3), typeof(GameObject) })]
        public class TooltipItemManagerPatch
        {
            public static void Prefix(Il2Cpp.TooltipItemManager __instance, Il2Cpp.ItemDataUnpacked data, Il2Cpp.TooltipItemManager.SlotType type, UnityEngine.Vector2 tooltipOffset, UnityEngine.Vector3 position, UnityEngine.GameObject opener)
            {
                FallenUtils.Log("OpenTooltip : ");
                originalLoreText = data.LoreText;
                HandleTooltipUpdate(data);
            }

            public static void Postfix(Il2Cpp.TooltipItemManager __instance, Il2Cpp.ItemDataUnpacked data, Il2Cpp.TooltipItemManager.SlotType type, UnityEngine.Vector2 tooltipOffset, UnityEngine.Vector3 position, UnityEngine.GameObject opener)
            {
                // Restore the original lore text after modification
                data.LoreText = originalLoreText;
            }

        }
    }
}



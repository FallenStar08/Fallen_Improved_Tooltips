using Fallen_LE_Mods.Shared;
using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods.Improved_Tooltips


{

    public class TooltipManager : MelonMod
    {
        private static string originalLoreText = "";
        private static ItemDataUnpacked? lastModifiedItem;
        private static void HandleTooltipUpdate(ItemDataUnpacked item)
        {
            Rule? match = FallenUtils.MatchFilterRule(item);

            bool addedFilterText = false;
            if (match != null && (ItemList.isEquipment(item.itemType) || ItemList.isIdol(item.itemType)))
            {
                var description = match.GetRuleDescription();

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
                string LPdescription = ownedLP > item.legendaryPotential ? $"with higher LP : {ownedLP}" :
                                       ownedLP < item.legendaryPotential ? $"with lower LP : {ownedLP}" :
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

        //Postfix(Il2Cpp.UITooltipItem __instance, Il2Cpp.UITooltipItem.ItemTooltipInfo __0, UnityEngine.Vector2 __1, UnityEngine.GameObject __2, Il2Cpp.ItemDataUnpacked __3, Il2Cpp.TooltipItemManager.SlotType __4)

        [HarmonyPatch(typeof(TooltipItemManager), "OpenItemTooltip", new Type[] { typeof(ItemDataUnpacked), typeof(TooltipItemManager.SlotType), typeof(Vector2), typeof(Vector3), typeof(GameObject), typeof(Vector2), })]
        public class TooltipItemManagerPatch
        {
            static void Prefix(
                    TooltipItemManager __instance,
                    ItemDataUnpacked data,
                    TooltipItemManager.SlotType type,
                    Vector2 _offset,
                    Vector3 position,
                    GameObject opener,
                    Vector2 openerSize)
            {
                if (data == null) return;

                originalLoreText = data.LoreText;
                lastModifiedItem = data;

                TooltipManager.HandleTooltipUpdate(data);
            }

            static void Postfix(
                TooltipItemManager __instance,
                ItemDataUnpacked data,
                TooltipItemManager.SlotType type,
                Vector2 _offset,
                Vector3 position,
                GameObject opener,
                Vector2 openerSize)
            {
                if (lastModifiedItem != null)
                {
                    lastModifiedItem.LoreText = originalLoreText;
                    lastModifiedItem = null;
                }
            }

        }
    }
}
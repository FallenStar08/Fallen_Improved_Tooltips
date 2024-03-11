using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods


{

    public class TooltipManager : MelonMod
    {
        private static void HandleTooltipUpdate(UITooltipItem.ItemTooltipInfo ttInfo, ItemDataUnpacked item)
        {
            Rule? match = FallenUtils.MatchFilterRule(item);

            bool addedFilterText = false;
            if (match != null && (ItemList.isEquipment(item.itemType) || ItemList.isIdol(item.itemType)))
            {

                //FallenUtils.Log("Rule match");
                var description = match.GetRuleDescription();
                if (description != null)
                {
                    if (ttInfo.loreText == "")
                    {
                        ttInfo.loreText += $"FilterRule : {description}";
                        addedFilterText = true;
                    }
                    else
                    {
                        ttInfo.loreText += $"\n\n</color>FilterRule : {description}";
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

                if (ttInfo.loreText == "")
                {
                    ttInfo.loreText += $"{description}";
                }
                else
                {
                    ttInfo.loreText += addedFilterText ? $"\n\n{description}" : $"\n\n</color>{description}";
                }
            }
            else if (item.isUniqueSetOrLegendary())
            {
                var description = $"Not Owned";
                if (ttInfo.loreText == "")
                {
                    ttInfo.loreText += $"{description}";
                }
                else
                {
                    ttInfo.loreText += $"\n\n</color>{description}";
                }
            }
        }

        [HarmonyPatch(typeof(UITooltipItem), "OpenTooltip", new Type[] { typeof(UITooltipItem.ItemTooltipInfo), typeof(UnityEngine.Vector2), typeof(UnityEngine.GameObject), typeof(ItemDataUnpacked), typeof(TooltipItemManager.SlotType) })]
        public class UITooltipItemPatch
        {
            public static void Prefix(ref UITooltipItem __instance, ref UITooltipItem.ItemTooltipInfo ttInfo, ref Vector2 position, ref GameObject targetSlot, ref ItemDataUnpacked _item, ref TooltipItemManager.SlotType slotType)
            {
                //FallenUtils.Log("OpenTooltip");
                HandleTooltipUpdate(ttInfo, _item);
            }
            //tooltipInfo.loreText
        }
        //OpenGroundTooltip(UITooltipItem.ItemTooltipInfo ttInfo, Vector2 position, GameObject targetSlot, ItemDataUnpacked _item)
        [HarmonyPatch(typeof(UITooltipItem), "OpenGroundTooltip", new Type[] { typeof(UITooltipItem.ItemTooltipInfo), typeof(UnityEngine.Vector2), typeof(UnityEngine.GameObject), typeof(ItemDataUnpacked) })]
        public class GroundUITooltipItemPatch
        {
            public static void Prefix(ref UITooltipItem __instance, ref UITooltipItem.ItemTooltipInfo ttInfo, ref Vector2 position, ref GameObject targetSlot, ref ItemDataUnpacked _item)
            {
                //FallenUtils.Log("GroundItemHoveredToolTip");
                HandleTooltipUpdate(ttInfo, _item);
            }

        }
    }
}



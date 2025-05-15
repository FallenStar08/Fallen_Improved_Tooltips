using System.Collections;
using Fallen_LE_Mods.Shared;
using Il2Cpp;
using Il2CppRewired.Utils;
using Il2CppTMPro;
using MelonLoader;

namespace Fallen_LE_Mods.Improved_Tooltips
{
    public class GroundLabelManager : MelonMod
    {

        public static class GroundLabelPatch
        {
            public static void Postfix(GroundItemLabel __instance)
            {
                //FallenUtils.Log("Postfix applied.");
                MelonCoroutines.Start(DelayRoutine(__instance));
            }

            private static IEnumerator DelayRoutine(GroundItemLabel item)
            {
                const string LabelMarker = "\u200B\u200B\u200B";  // Invisible marker to track changes
                yield return null; // Ensure this runs after other postfixes

                if (!item) yield break;

                ItemDataUnpacked itemDataUnpacked;
                try
                {
                    itemDataUnpacked = item.getItemData();
                    if (itemDataUnpacked.IsNullOrDestroyed()) yield break;
                }
                catch
                {
                    yield break;
                }

                TextMeshProUGUI itemText = item.itemText;
                if (!itemText) yield break;

                string currentText = itemText.text;

                bool isModified = currentText.Contains(LabelMarker);

                if (!isModified)
                {
                    if (itemDataUnpacked.isUnique())
                    {
                        string newSuffix = "";

                        ItemDataUnpacked matchedUniqueOrSet = FallenUtils.FindSimilarUniqueItemInStash(itemDataUnpacked);
                        if (matchedUniqueOrSet != null)
                        {
                            int ownedLP = matchedUniqueOrSet.legendaryPotential;
                            newSuffix = ownedLP > itemDataUnpacked.legendaryPotential ? " <color=#FF0000>↓</color>" :
                                        ownedLP < itemDataUnpacked.legendaryPotential ? " <color=#00FF00>↑</color>" :
                                                                                        " <color=#0000FF>=</color>";
                        }

                        else if (itemDataUnpacked.isUniqueSetOrLegendary())
                        {
                            newSuffix += " <i><color=#FFD700>NEW</color></i>";
                        }

                        string newFullText = currentText + newSuffix + LabelMarker;

                        // Only update if the full text is different from the current
                        if (itemText.text != newFullText)
                        {
                            itemText.text = item.emphasized ? newFullText.ToUpper() : newFullText;
                            //item.sceneFollower?.calculateDimensions();
                        }
                    }
                }
            }
        }
    }
}

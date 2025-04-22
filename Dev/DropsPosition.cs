#if RELEASE
using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Fallen_LE_Mods.Dev

{
    [HarmonyPatch(typeof(GroundItemManager), "dropItemForPlayer")]
    public class ItemDropHandler : MelonMod
    {
        public static void Prefix(ref GroundItemManager __instance, ref Actor player, ref ItemData itemData, ref Vector3 location, ref bool playDropSound)
        {
            if (ItemList.isCraftingItem(itemData.itemType))
            {
                Vector3 playerPosition = player.position();
                location = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            }

        }
    }

    [HarmonyPatch(typeof(GroundItemManager), "dropGoldForPlayer", new Type[] { typeof(Actor), typeof(int), typeof(Vector3), typeof(bool) })]
    public class GoldDropHandler : MelonMod
    {

        public static void Prefix(ref GroundItemManager __instance, ref Actor player, ref int goldValue, ref Vector3 location, ref bool playDropSound)
        {
            Vector3 playerPosition = player.position();
            location = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            playDropSound = false;

        }
    }
    [HarmonyPatch(typeof(GroundItemManager), "dropXPTomeForPlayer")]
    public class XPTomeDropHandler : MelonMod
    {
        public static void Prefix(ref GroundItemManager __instance, ref Actor player, ref int experience, ref Vector3 location, ref bool playDropSound)
        {
            Vector3 playerPosition = player.position();
            location = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);

        }
    }

}
#endif
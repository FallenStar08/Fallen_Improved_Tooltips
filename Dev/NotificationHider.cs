using HarmonyLib;
using Il2Cpp;

namespace Fallen_LE_Mods.Dev
{
    public class NotificationHider
    {
        [HarmonyPatch(typeof(Notifications), "MaterialAdded")]
        public class Notifications_MaterialAdded
        {
            [HarmonyPrefix]
            static bool Prefix(string __0)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(Notifications), "RuneOrGlyphAddedNotification")]
        public class Notifications_RuneOrGlyphAddedNotification
        {
            [HarmonyPrefix]
            static bool Prefix(string __0, int __1)
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(Notifications), "ShardAdded")]
        public class Notifications_ShardAdded
        {
            [HarmonyPrefix]
            static bool Prefix(int __0, int __1)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(Notifications), "MultiShardsAdded")]
        public class Notifications_MultiShardsAdded
        {
            [HarmonyPrefix]
            static bool Prefix(int __0)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(Notifications), "showBufferedCraftingNotifications")]
        public class Notifications_showBufferedCraftingNotifications
        {
            [HarmonyPrefix]
            static bool Prefix(ref Notifications __instance)
            {
                return false;
            }
        }
    }
}


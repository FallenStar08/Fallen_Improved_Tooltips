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
        private static long _lastStoreMaterialsTicks = 0;
        private static bool _callQueued = false;
        private static bool _storeRequested = false;
        private static float _queuedDelay = 0f;
        private static Actor? _player;

        public override void OnUpdate()
        {
            if (_callQueued)
            {
                _queuedDelay -= Time.deltaTime * 1000f;

                if (_queuedDelay <= 0f)
                {
                    _lastStoreMaterialsTicks = DateTime.UtcNow.Ticks;
                    _callQueued = false;

                    if (_storeRequested && _player != null)
                    {
                        _storeRequested = false;
                        ItemContainersManager.Instance.TryStoreMaterials(_player);
                    }
                }
            }
        }

        public static bool Prefix(GroundItemManager __instance, Actor player, ItemData itemData, Vector3 location, bool playDropSound)
        {
            if (!ItemList.isCraftingItem(itemData.itemType) && !ItemList.isWovenEcho(itemData.itemType) && !Item.isKey(itemData.itemType))
                return true;

            Vector3 playerPosition = player.position();
            location = playerPosition;

            if (!ItemContainersManager.Instance.attemptToPickupItem(itemData, playerPosition))
                return true;

            HandleStoreMaterials(player);
            return false;
        }

        private static void HandleStoreMaterials(Actor player)
        {
            long nowTicks = DateTime.UtcNow.Ticks;
            long elapsedMs = (nowTicks - _lastStoreMaterialsTicks) / TimeSpan.TicksPerMillisecond;

            _player = player;
            _storeRequested = true;

            if (elapsedMs >= 100)
            {
                _lastStoreMaterialsTicks = nowTicks;
                _storeRequested = false;
                ItemContainersManager.Instance.TryStoreMaterials(player);
            }
            else if (!_callQueued)
            {
                _callQueued = true;
                _queuedDelay = 100f - elapsedMs;
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
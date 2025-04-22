using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using Il2CppLE.UI;

namespace Fallen_LE_Mods.Shared
{
    //Probably no need to refresh these on each load but idk...
    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class GameReferencesCache
    {
        public static ItemFilterManager? myManager;
        public static ItemContainersManager? myItemContainer;
        public static Il2CppSystem.Collections.Generic.List<ItemContainer>? myStash;
        public static void Postfix(ref LoadingScreen __instance)
        {
            myManager = FallenUtils.GetFilterManager;
            myItemContainer = ItemContainersManager.Instance;
            myStash = StashTabbedUIControls.instance.container.containers;
        }
    }
}

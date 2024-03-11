using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using Il2CppLE.UI;

namespace Fallen_LE_Mods
{
    //We probably don't need to refresh them on each loading screen
    //But I'm not familiar enough with unity to be sure
    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class ThingsKeeper
    {
        public static ItemFilterManager? myManager;
        public static ItemContainersManager? myItemContainer;
        public static TabbedItemContainer? myStash;
        public static void Postfix(ref LoadingScreen __instance)
        {
            myManager = FallenUtils.GetFilterManager;
            myItemContainer = ItemContainersManager.instance;
            myStash = myItemContainer.stash;
        }
    }
}

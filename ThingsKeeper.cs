using Il2CppLE.UI;
using HarmonyLib;
using Il2CppItemFiltering;
using Il2Cpp;

namespace Fallen_LE_Mods
{

    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class ThingsKeeper
    {
        public static ItemFilterManager? myManager;
        public static ItemContainersManager? myItemContainer;
        public static TabbedItemContainer? myStash;
        public static void Postfix(ref LoadingScreen __instance)
        {
            myManager=FallenUtils.GetFilterManager;
            myItemContainer = ItemContainersManager.instance;
            myStash = myItemContainer.stash;
        }
    }
}

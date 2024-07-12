using HarmonyLib;
using Il2Cpp;
using Il2CppItemFiltering;
using Il2CppLE.UI;

namespace Fallen_LE_Mods
{
    //Probably no need to refresh these on each load but idk...
    [HarmonyPatch(typeof(LoadingScreen), "Disable")]
    public class ThingsKeeper
    {
        public static ItemFilterManager? myManager;
        public static ItemContainersManager? myItemContainer;
        public static Il2CppSystem.Collections.Generic.List<Il2Cpp.ItemContainer>? myStash;
        public static void Postfix(ref LoadingScreen __instance)
        {
            myManager = FallenUtils.GetFilterManager;
            myItemContainer = ItemContainersManager.instance;
            myStash = StashTabbedUIControls.instance.container.containers;
            //FallenUtils.Log($"myManager {myManager} myItemContainer {myItemContainer} myStash {myStash}");
        }
    }
}

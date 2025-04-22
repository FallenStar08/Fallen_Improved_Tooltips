using Fallen_LE_Mods.Dev;
using MelonLoader;


//PlayerFinder.getPlayerActor()

namespace Fallen_LE_Mods
{
    public class MyMod : MelonMod
    {
#if RELEASE
        private static string[] pauseScenes = { "M_Rest", "ClientSplash", "PersistentUI", "Login", "CharacterSelectScene", "EoT", "MonolithHub", "Bazaar", "Observatory" };

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (pauseScenes.Contains(sceneName))
            {
                GameStatsTracker.Pause();
            }
            else
            {
                GameStatsTracker.Resume();
            }

        }
#endif
    }

}
using BeatBoards.Utilities;
using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BeatBoards
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static HarmonyInstance harmony;

        public void Init(IPALogger logger)
        {
            Logger.Log = logger;
        }

        public void OnApplicationStart()
        {
            harmony = HarmonyInstance.Create("com.auros.BeatSaber.BeatBoards");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
           
        }

        public void OnApplicationQuit()
        {
        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "EmptyTransition")
            {
                _ = Core.Events.Instance;
                _ = UI.LeaderboardUIManager.Instance;
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}

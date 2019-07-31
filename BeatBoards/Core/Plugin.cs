using BeatBoards.UI;
using BeatBoards.Utilities;
using CustomUI.MenuButton;
using Harmony;
using IPA;
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
                _ = LeaderboardUIManager.Instance;
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MenuCore")
            {
                MenuButtonUI.AddButton("Beat Boards", BeatBoardsMenu.Load);
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}

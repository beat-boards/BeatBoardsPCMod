using BeatBoards.UI;
using BeatBoards.UI.FlowCoordinators;
using BeatBoards.Utilities;
using CustomUI.BeatSaber;
using CustomUI.MenuButton;
using CustomUI.Utilities;
using Harmony;
using IPA;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BeatBoards
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static HarmonyInstance harmony;

        public void Init(IPALogger logger)
        {
            Utilities.Logger.Log = logger;
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
                _ = BeatBoardsUIManager.Instance;
            }
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MenuCore")
            {
                MenuButtonUI.AddButton("Beat Boards", BeatBoardsUIManager.Instance.BeatBoardsButtonPressed);
                BeatBoardsUIManager.Instance.AddVotingButtons();
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = BeatBoards.Utilities.Logger;
using UnityEngine.Events;

namespace BeatBoards.Core
{
    public class Events : MonoBehaviour
    {
        private static Events _instance;

        public static Events Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Debug("Initializing: BeatBoards Events");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Events Singleton");
                    _instance = eventsGameObject.AddComponent<Events>();
                    DontDestroyOnLoad(eventsGameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }

        public UnityEvent<IPreviewBeatmapLevel, PlatformLeaderboardViewController> songSelected;

        public void Init()
        {
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        public Action<IDifficultyBeatmap, LeaderboardTableView> leaderboardOpened;

        private void ActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameCore")
            {

            }
            if (newScene.name == "MenuCore")
            {
                
            }
        }
    }
}

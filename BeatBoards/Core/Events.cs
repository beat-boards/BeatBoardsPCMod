using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = BeatBoards.Utilities.Logger;
using UnityEngine.Events;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

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
                    Logger.Log.Info("Initializing: BeatBoards Events");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Events Singleton");
                    _instance = eventsGameObject.AddComponent<Events>();
                    DontDestroyOnLoad(eventsGameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }

        public void Init()
        {
            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        public void OnDisable()
        {
            SceneManager.activeSceneChanged -= ActiveSceneChanged;
        }

        public Action IDSet;
        public Action levelStarted;
        public Action<IDifficultyBeatmap, LeaderboardTableView> leaderboardOpened;
        

        private void ActiveSceneChanged(Scene oldScene, Scene newScene)
        {

        }
    }
}

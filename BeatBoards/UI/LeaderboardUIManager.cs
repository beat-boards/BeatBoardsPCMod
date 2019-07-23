using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = BeatBoards.Utilities.Logger;
using BeatBoards.Core;
using IPA.Utilities;
using UnityEngine.Events;

namespace BeatBoards.UI
{
    public class LeaderboardUIManager : MonoBehaviour
    {
        private static LeaderboardUIManager _instance;

        public static LeaderboardUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Debug("Initializing: BeatBoards Leaderboard UI Manager");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Events Singleton");
                    _instance = eventsGameObject.AddComponent<LeaderboardUIManager>();
                    DontDestroyOnLoad(eventsGameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }

        Events eventManager;

        public void Init()
        {
            eventManager = Events.Instance;
            eventManager.leaderboardOpened += LeaderboardOpened_Event;
            
        }

        private void LeaderboardOpened_Event(IDifficultyBeatmap arg1, LeaderboardTableView arg2)
        {
            Logger.Log.Info("LEADERBOARD OPENED HELL YEAH");

            List<LeaderboardTableView.ScoreData> scoreData = new List<LeaderboardTableView.ScoreData>() { };
            scoreData.Add(new LeaderboardTableView.ScoreData(69420, "your mom lmao", 1, true));

            arg2.SetScores(scoreData, 1);
        }
    }
}

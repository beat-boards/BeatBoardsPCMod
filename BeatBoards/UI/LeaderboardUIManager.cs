using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = BeatBoards.Utilities.Logger;
using BeatBoards.Core;
using CustomUI.Utilities;

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
        private Sprite _PCIcon;
        public Sprite PCIcon { get { if (_PCIcon == null) { _PCIcon = UIUtilities.LoadSpriteFromResources("BeatBoards.Media.icon_pc1.png"); } return _PCIcon; } }
        List<string> varioususernames = new List<string>() { "Taichi", "LogantheobaldRank5InTheWorld", "Auros", "Assistant", "Megalon", "elliottate", "Klouder", "OrangeW", "Umbranox", "joelseph", "Beige", "Range", "Sam", "DeeJay", "andruzzzhka", "Arti", "DaNike", "emulamer", "halsafar", "ikeiwa", "monkeymanboy", "Moon", "Nova", "raftario", "Ruu | LIV", "ragesaq darth maul", "Reaxt", "Thanos" };

        public void Init()
        {
            eventManager = Events.Instance;
            eventManager.leaderboardOpened += LeaderboardOpened_Event;
            _ = PCIcon;
            Logger.Log.Warn(PCIcon.ToString());
        }
        
        private List<LeaderboardTableView.ScoreData> RandomLeaderboardData()
        {
            var randomizedorder = varioususernames.OrderBy(a => Guid.NewGuid()).ToList();
            List<LeaderboardTableView.ScoreData> scoreData = new List<LeaderboardTableView.ScoreData>() { };
            int rank = 1;
            foreach (var username in randomizedorder)
            {
                var randomScore = UnityEngine.Random.Range(200000, 1250000);
                var percent = Math.Round((randomScore / 1300000f * 100), 1);
                var randomRank = UnityEngine.Random.Range(1, 950);

                bool fc = false;
                if (UnityEngine.Random.Range(1f, 13f) == 4)
                    fc = true;

                scoreData.Add(new LeaderboardTableView.ScoreData(randomScore, $"{username} <size=70%>(<color=#8800ff>{percent}%</color> - <color=#00ffff>{percent * 1.3}RP</color>)</size><size=40%> Global: {randomRank}</size>", rank, fc));
                rank++;
            }
            return scoreData;
        }

        private void LeaderboardOpened_Event(IDifficultyBeatmap arg1, LeaderboardTableView arg2)
        {
            List<LeaderboardTableView.ScoreData> scoreData = new List<LeaderboardTableView.ScoreData>() { };
            var scda = RandomLeaderboardData().OrderByDescending(a => a.score).ToList();
            int rank = 1;
            foreach (var data in scda)
            {
                scoreData.Add(new LeaderboardTableView.ScoreData(data.score, data.playerName, rank, data.fullCombo));
                rank++;
            }
            arg2.SetScores(scoreData, 100);
        }
    }
}

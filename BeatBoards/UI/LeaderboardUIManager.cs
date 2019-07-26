using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = BeatBoards.Utilities.Logger;
using BeatBoards.Core;
using CustomUI.Utilities;
using UnityEngine.UI;
using CustomUI.BeatSaber;
using CustomUI.MenuButton;
using System.IO;
using BeatBoards.Replays;
using Newtonsoft.Json;
using HMUI;
using VRUI;

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
                    Logger.Log.Info("Initializing: BeatBoards Leaderboard UI Manager");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Leaderboard UI Manager Singleton");
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
        List<string> varioususernames = new List<string>() { "Taichi", "Logantheobald, Rank 5 in the world on Beat Saber", "Auros", "Assistant", "Megalon", "elliottate", "Klouder", "OrangeW", "Umbranox", "joelseph", "Beige", "Range", "Sam", "DeeJay", "andruzzzhka", "Arti", "DaNike", "emulamer", "halsafar", "ikeiwa", "monkeymanboy", "Moon", "Nova", "raftario", "Ruu | LIV", "ragesaq darth maul", "Reaxt", "Thanos" };
        public Button replaysButton;
        public string currentlySelectedReplay;
        IDifficultyBeatmap currentlySelectedBeatmap;

        public void Init()
        {
            eventManager = Events.Instance;
            eventManager.leaderboardOpened += LeaderboardOpened_Event;
            _ = PCIcon;
        }

        public void OnDisable()
        {
            eventManager.leaderboardOpened -= LeaderboardOpened_Event;
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

                scoreData.Add(new LeaderboardTableView.ScoreData(randomScore, $"{username} <size=70%>(<color=#bf42f5>{percent}%</color> - <color=#00ffff>{percent * 1.3}RP</color>)</size><size=40%> Global: {randomRank}</size>", rank, fc));
                rank++;
            }
            return scoreData;
        }

        private void LeaderboardOpened_Event(IDifficultyBeatmap arg1, LeaderboardTableView arg2)
        {
            currentlySelectedBeatmap = arg1;
            List<LeaderboardTableView.ScoreData> scoreData = new List<LeaderboardTableView.ScoreData>() { };
            var scda = RandomLeaderboardData().OrderByDescending(a => a.score).ToList();
            int rank = 1;
            foreach (var data in scda)
            {
                scoreData.Add(new LeaderboardTableView.ScoreData(data.score, data.playerName, rank, data.fullCombo));
                rank++;
            }
            arg2.SetScores(scoreData, 100);


            ReplayManager.Instance.currentLevelHash = currentlySelectedBeatmap.level.levelID;
            ReplayManager.Instance.currentDifficulty = currentlySelectedBeatmap.difficulty.ToString();
            ReplayManager.Instance.selectedReplay = currentlySelectedReplay;
        }

        public void ReplayMenu()
        {
            var replaySelectionMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Replays");
            var viewController = BeatSaberUI.CreateViewController<CustomListViewController>();
            replaySelectionMenu.SetRightViewController(viewController, true, (firstActivation, type) =>
            {
                List<PositionDataSet> posDataSet = new List<PositionDataSet>() { };
                if (Directory.Exists(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentlySelectedBeatmap.level.levelID))
                {
                    foreach (string file in Directory.GetFiles(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentlySelectedBeatmap.level.levelID))
                    {
                        posDataSet.Add(JsonConvert.DeserializeObject<PositionDataSet>(File.ReadAllText(file)));
                    }
                    foreach (PositionDataSet posDataS in posDataSet)
                    {
                        if (posDataS.Difficulty == currentlySelectedBeatmap.difficulty.ToString())
                            viewController.Data.Add(new CustomCellInfo(posDataS.Hash, posDataS.Date, PCIcon));
                    }
                }
                else
                {
                    viewController.CreateText("No Replays Found", new Vector2(0, 0));
                }

                viewController._customListTableView.ReloadData();
                viewController._customListTableView.didSelectCellWithIdxEvent += (tV, id) =>
                {
                    currentlySelectedReplay = posDataSet[id].Hash;
                };

                var button = viewController.CreateUIButton("OkButton", new Vector2(35f, 20f));
                button.onClick.AddListener(delegate { ReplayManager.Instance.recording = true; ReplayManager.Instance.playback = false; });
                button.SetButtonText("Record");
                button.ToggleWordWrapping(false);
                button.SetButtonTextSize(3);

                var button2 = viewController.CreateUIButton("OkButton", new Vector2(35f, 0f));
                button2.onClick.AddListener(delegate { ReplayManager.Instance.recording = false; ReplayManager.Instance.playback = true; });
                button2.SetButtonText("Playback");
                button2.ToggleWordWrapping(false);
                button2.SetButtonTextSize(3);

                var button3 = viewController.CreateUIButton("OkButton", new Vector2(35f, -20f));
                button3.onClick.AddListener(delegate { ReplayManager.Instance.recording = false; ReplayManager.Instance.playback = false; });
                button3.SetButtonText("Neither");
                button3.ToggleWordWrapping(false);
                button3.SetButtonTextSize(3);

            });
            replaySelectionMenu.Present();
        }
    }
}

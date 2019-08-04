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

using Newtonsoft.Json;
using HMUI;
using VRUI;
using UnityEngine.Networking;
using System.Collections;
using BeatBoards.Utilities;
using System.Threading.Tasks;
using BeatBoards.UI.FlowCoordinators;
using TMPro;

namespace BeatBoards.UI
{
    public class BeatBoardsUIManager : MonoBehaviour
    {
        private static BeatBoardsUIManager _instance;
        public static BeatBoardsUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Info("Initializing: BeatBoards Leaderboard UI Manager");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Leaderboard UI Manager Singleton");
                    _instance = eventsGameObject.AddComponent<BeatBoardsUIManager>();
                    DontDestroyOnLoad(eventsGameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }

        Events eventManager;
        private Sprite _PCIcon;
        private Sprite _upvoteIcon;
        private Sprite _downvoteIcon;
        public Sprite PCIcon { get { if (_PCIcon == null) { _PCIcon = UIUtilities.LoadSpriteFromResources("BeatBoards.Media.icon_pc1.png"); } return _PCIcon; } }
        public Sprite UpvoteIcon { get { if (_upvoteIcon == null) { _upvoteIcon = UIUtilities.LoadSpriteFromResources("BeatBoards.Media.Upvote.png"); _upvoteIcon.texture.wrapMode = TextureWrapMode.Clamp; } return _upvoteIcon; } }
        public Sprite DownvoteIcon { get { if (_downvoteIcon == null) { _downvoteIcon = UIUtilities.LoadSpriteFromResources("BeatBoards.Media.Downvote.png"); _downvoteIcon.texture.wrapMode = TextureWrapMode.Clamp; } return _downvoteIcon; } }
        List<string> varioususernames = new List<string>() { "Taichi", "Logantheobald, Rank 5 in the world on Beat Saber", "Auros", "Assistant", "Megalon", "elliottate", "Klouder", "OrangeW", "Umbranox", "joelseph", "Beige", "Range", "Sam", "DeeJay", "andruzzzhka", "Arti", "DaNike", "emulamer", "halsafar", "ikeiwa", "monkeymanboy", "Moon", "Nova", "raftario", "Ruu | LIV", "ragesaq darth maul", "Reaxt", "Thanos" };
        public Button replaysButton;
        public string currentlySelectedReplay;
        LeaderboardTableView currentLeaderboard;
        IDifficultyBeatmap currentlySelectedBeatmap;
        public APIModels.LiteMap mapData = new APIModels.LiteMap() { };
        public List<APIModels.Score> scores = new List<APIModels.Score>() { };
        public Dictionary<APIModels.Score, APIModels.User> userScoreDictionary = new Dictionary<APIModels.Score, APIModels.User>() { };
        private Button _upvoteButton;
        private Button _downvoteButton;
        private TextMeshProUGUI _ratingText;

        IEnumerator SetID(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                Global.BeatBoardsID = "0";
            }
            else
            {
                APIModels.User user = JsonConvert.DeserializeObject<APIModels.User[]>(www.downloadHandler.text).First();
                if (user.BeatBoardsID.Length > 3)
                {
                    Global.BeatBoardsID = user.BeatBoardsID;
                }
                    
            }
        }

        IEnumerator GetMapData(string levelhash, BeatmapDifficulty difficulty) //TODO Phase out and calculate map data locally
        {
            UnityWebRequest www = UnityWebRequest.Get("http://beatboards.net/api/litemap?levelhash=" + levelhash + "&difficulty=" + difficulty);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                yield return new WaitForSeconds(.05f);
            }
            else
            {
                mapData = JsonConvert.DeserializeObject<List<APIModels.LiteMap>>(www.downloadHandler.text).First();
                UnityWebRequest www2 = UnityWebRequest.Get("http://beatboards.net/api/scores?maphash=" + mapData.MapHash);
                yield return www2.SendWebRequest();

                if (www2.isNetworkError || www2.isHttpError)
                {
                    Logger.Log.Error(www2.error);
                    yield return new WaitForSeconds(.05f);
                }
                else
                {
                    scores = JsonConvert.DeserializeObject<List<APIModels.Score>>(www2.downloadHandler.text);
                    userScoreDictionary = new Dictionary<APIModels.Score, APIModels.User>();
                    foreach (var score in scores)
                    {
                        Logger.Log.Info("http://beatboards.net/api/users?platformID=" + score.UserID);
                        UnityWebRequest www3 = UnityWebRequest.Get("http://beatboards.net/api/users?platformID=" + score.UserID);
                        yield return www3.SendWebRequest();

                        if (www3.isNetworkError || www3.isHttpError)
                        {
                            Logger.Log.Error(www3.error);
                            yield return new WaitForSeconds(.05f);
                        }
                        else
                        {
                            userScoreDictionary.Add(score, JsonConvert.DeserializeObject<List<APIModels.User>>(www3.downloadHandler.text).First());
                        }
                    }
                    ProcessData();
                }
            }
        }

        public void ProcessData()
        {
            
            List<LeaderboardTableView.ScoreData> lscoreData = new List<LeaderboardTableView.ScoreData>();
            foreach (var dat in userScoreDictionary)
            {
                lscoreData.Add(new LeaderboardTableView.ScoreData(dat.Key.AdjustedScore, $"{dat.Value.UserData.Nickname} <size=70%>(<color=#bf42f5>{dat.Key.RawPercent}%</color> - <color=#00ffff>{Math.Round(dat.Key.RawPercent * 1.3, 2)}RP</color>)</size><size=40%> Global: {dat.Value.Rank}</size>", 0, false));
            }
            lscoreData = lscoreData.OrderByDescending(o => o.score).ToList();
            List<LeaderboardTableView.ScoreData> ldisplayData = new List<LeaderboardTableView.ScoreData>();
            int rank = 1;
            foreach (var dat in lscoreData)
            {
                ldisplayData.Add(new LeaderboardTableView.ScoreData(dat.score, dat.playerName, rank, false));
                rank++;
            }

            Logger.Log.Info("e");

            currentLeaderboard.SetScores(ldisplayData, -1);
        }

        public void Init()
        {
            eventManager = Events.Instance;
            StartCoroutine(SetID("http://beatboards.net/api/users?platformID=" + BS_Utils.Gameplay.GetUserInfo.GetUserID()));
            eventManager.leaderboardOpened += LeaderboardOpened_Event;
            BSEvents.levelFailed += LevelFailed;
            BSEvents.levelCleared += LevelCleared;
            _ = PCIcon;
            
        }

        public void OnDisable()
        {
            eventManager.leaderboardOpened -= LeaderboardOpened_Event;
            BSEvents.levelFailed -= LevelFailed;
            BSEvents.levelCleared -= LevelCleared;
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
            StopAllCoroutines();

            currentLeaderboard = arg2;
            StartCoroutine(GetMapData(arg1.level.levelID, arg1.difficulty));
            currentlySelectedBeatmap = arg1;

        }

        public void AddVotingButtons()
        {
            ResultsViewController resultsViewController = Resources.FindObjectsOfTypeAll<ResultsViewController>().First(x => x.name == "StandardLevelResultsViewController");

            _upvoteButton = resultsViewController.CreateUIButton("PracticeButton", new Vector2(-65f, 10f), new Vector2(12f, 12f), () => { VoteForSong(true); }, "", UpvoteIcon);
            _downvoteButton = resultsViewController.CreateUIButton("PracticeButton", new Vector2(-65f, -10f), new Vector2(12f, 12f), () => { VoteForSong(false); }, "", DownvoteIcon);
            _ratingText = resultsViewController.CreateText("0", new Vector2(-36.5f, 0f));
            _ratingText.alignment = TextAlignmentOptions.Center;
            _ratingText.fontSize = 7f;
            _ratingText.lineSpacing = -38f;

        }

        private void VoteForSong(bool vote)
        {
            Logger.Log.Info(vote.ToString());
        }

        private void LevelCleared(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {

        }

        private void LevelFailed(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {

        }

        // other thing

        public BeatBoardsMenuFlowCoordinator bbmFlowCoordinator;

        public void BeatBoardsButtonPressed()
        {
            if (bbmFlowCoordinator == null)
                bbmFlowCoordinator = new GameObject("BeatBoards: Flow Coordinator").AddComponent<BeatBoardsMenuFlowCoordinator>();
            MainFlowCoordinator mainFlow = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            mainFlow.InvokeMethod("PresentFlowCoordinator", bbmFlowCoordinator, null, false, false);
        }
    }
}
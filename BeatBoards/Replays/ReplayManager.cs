using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeatBoards.Core;
using Logger = BeatBoards.Utilities.Logger;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using BeatBoards.UI;
using CustomUI.Utilities;

namespace BeatBoards.Replays
{
    public class ReplayManager : MonoBehaviour
    {
        private static ReplayManager _instance;
        public static ReplayManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Info("Initializing: BeatBoards Replay Manager");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Replay Manager Singleton");
                    _instance = eventsGameObject.AddComponent<ReplayManager>();
                    DontDestroyOnLoad(eventsGameObject);
                    _instance.Init();
                }
                return _instance;
            }
        }

        PlayerController _playerController;
        PlayerController playerController
        {
            get
            {
                if (_playerController == null)
                    _playerController = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();

                return _playerController;
            }
        }

        AudioTimeSyncController _audioTimeSyncController;
        public AudioTimeSyncController audioTimeSyncController
        {
            get
            {
                if (_audioTimeSyncController == null)
                    _audioTimeSyncController = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();

                return _audioTimeSyncController;
            }
        }
        public Events eventsManager;
        public List<PositionData> positionData = new List<PositionData>() { };
        public List<PositionData> playBackData;
        public Dictionary<float, PositionData> posDictionary = new Dictionary<float, PositionData>() { };
        public bool gameObjectActive = false;
        public bool recording = false;
        public bool playback = false;
        public string currentLevelHash;
        public string currentDifficulty;
        public string selectedReplay;
        

        private void Init()
        {
            eventsManager = Events.Instance;
            eventsManager.levelStarted += LevelStarted;
        }

        private void OnDisable()
        {
            gameObjectActive = false;
            eventsManager.levelStarted -= LevelStarted;

            if (recording == true)
            {
                var uniqueList = positionData.GroupBy(x => x.SongTime).Select(y => y.First()).ToList();
                var finalData = new PositionDataSet() { Date = DateTime.Now.ToString(), Difficulty = currentDifficulty, Hash = uniqueList.GetHashCode().ToString(), PositionData = uniqueList };
                if (!Directory.Exists(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentLevelHash))
                    Directory.CreateDirectory(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentLevelHash);
                File.WriteAllText(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentLevelHash + "/ReplayData_" + uniqueList.GetHashCode().ToString() + ".json", JsonConvert.SerializeObject(finalData));
            }
        }
        private void LevelStarted()
        {

            if (playback == true)
            {
                var playBackInfo = JsonConvert.DeserializeObject<PositionDataSet>(File.ReadAllText(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/" + currentLevelHash + "/ReplayData_" + LeaderboardUIManager.Instance.currentlySelectedReplay + ".json"));
                playBackData = playBackInfo.PositionData;
                //Logger.Log.Info(playBackInfo.PositionData.Count().ToString());
                //foreach (PositionData posDat in playBackData)
                //{
                //    posDictionary.Add(posDat.SongTime, posDat);
                //    Logger.Log.Info(posDat.SongTime.ToString());
                //}
            }

            gameObjectActive = true;
        }
    }

    public class PositionDataSet
    {
        public string Date { get; set; }
        public string Difficulty { get; set; }
        public string Hash { get; set; }
        public List<PositionData> PositionData { get; set; }
    }

    public class PositionData
    {
        public float SongTime { get; set; }
        public SaberData LeftSaber { get; set; }
        public SaberData RightSaber { get; set; }
        public HeadData Head { get; set; }
    }

    public class SaberData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }
    public class HeadData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }
}

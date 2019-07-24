using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeatBoards.Core;
using Logger = BeatBoards.Utilities.Logger;
using System.Collections;
using Newtonsoft.Json;
using System.IO;

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
        private void Init()
        {
            eventsManager = Events.Instance;
            eventsManager.levelStarted += LevelStarted;
        }

        public List<PositionData> playBackData;
        public Dictionary<float, PositionData> posDictionary = new Dictionary<float, PositionData>() { };

        private void OnDisable()
        {
            gameObjectActive = false;
            eventsManager.levelStarted -= LevelStarted;

            if (recording == true)
            {
                var uniqueList = positionData.GroupBy(x => x.SongTime).Select(y => y.First()).ToList();
                File.WriteAllText(Environment.CurrentDirectory.Replace('\\', '/') + "/ReplayData_" + uniqueList.GetHashCode() + ".json", JsonConvert.SerializeObject(uniqueList));
            }

            

        }
        private void LevelStarted()
        {
            if (recording == true)
            {
                SharedCoroutineStarter.instance.StartCoroutine(UpdateSongTime());
            }

            if (playback == true)
            {
                playBackData = JsonConvert.DeserializeObject<List<PositionData>>(File.ReadAllText(Environment.CurrentDirectory.Replace('\\', '/') + "/ReplayData_1953971712.json"));

                foreach (PositionData posDat in playBackData)
                {
                    posDictionary.Add(posDat.SongTime, posDat);
                    Logger.Log.Info(posDat.SongTime.ToString());
                }
            }

            gameObjectActive = true;
        }
        
        public bool gameObjectActive = false;
        bool recording = false;
        public bool playback = false;
        //float counter = 0f;

        public IEnumerator UpdateSongTime()
        {
            Vector3 leftSaberPos = playerController.leftSaber.transform.position;
            Vector3 leftSaberRot = playerController.leftSaber.transform.rotation.eulerAngles;
            Vector3 rightSaberPos = playerController.rightSaber.transform.position;
            Vector3 rightSaberRot = playerController.rightSaber.transform.rotation.eulerAngles;

            float t = 0;
            float songLength = audioTimeSyncController.songEndTime;

            while (t < songLength)
            {
                positionData.Add(new PositionData()
                {
                    SongTime = (float)Math.Round(audioTimeSyncController.songTime, 2),
                    LeftSaber = new SaberData()
                    {
                        PositionX = leftSaberPos.x,
                        PositionY = leftSaberPos.y,
                        PositionZ = leftSaberPos.z,
                        RotationX = leftSaberRot.x,
                        RotationY = leftSaberRot.y,
                        RotationZ = leftSaberRot.z
                    },
                    RightSaber = new SaberData()
                    {
                        PositionX = rightSaberPos.x,
                        PositionY = rightSaberPos.y,
                        PositionZ = rightSaberPos.z,
                        RotationX = rightSaberRot.x,
                        RotationY = rightSaberRot.y,
                        RotationZ = rightSaberRot.z
                    }
                    
                });

                
                //t = audioTimeSyncController.songTime;
                yield return new WaitForSeconds(.01f);
            }
        }

        private IEnumerator Playback()
        {
            float t = 0;
            float songLength = audioTimeSyncController.songEndTime;

            while (t < songLength)
            {
                //(float)Math.Round(audioTimeSyncController.songTime, 2)


                yield return new WaitForSeconds(.01f);
            }
        }

        private void LateUpdate()
        {
            if (recording == true)
            {
                
                
            }
            else if (playback == true && recording == true)
            {
                if (ReplayManager.Instance.playback == true && ReplayManager.Instance.gameObjectActive == true)
                {
                    float songTime = ReplayManager.Instance.audioTimeSyncController.songTime;

                    PositionData posDat = null;
                    bool axe = ReplayManager.Instance.posDictionary.TryGetValue(songTime, out posDat);

                    if (axe == true)
                    {
                        Logger.Log.Warn("Okay, this is epic.");
                    }
                }
            }
            
        }
    }
    
    public class PositionData
    {
        public float SongTime { get; set; }
        public SaberData LeftSaber { get; set; }
        public SaberData RightSaber { get; set; }
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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using BeatBoards.Core;
//using Logger = BeatBoards.Utilities.Logger;
//using System.Collections;
//using Newtonsoft.Json;
//using System.IO;
//using BeatBoards.UI;
//using CustomUI.Utilities;
//using System.Runtime.Serialization.Formatters.Binary;

//namespace BeatBoards.Replays
//{
//    public class ReplayManager : MonoBehaviour
//    {
//        private static ReplayManager _instance;
//        public static ReplayManager Instance
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    Logger.Log.Info("Initializing: BeatBoards Replay Manager");
//                    GameObject eventsGameObject = new GameObject("BeatBoards: Replay Manager Singleton");
//                    _instance = eventsGameObject.AddComponent<ReplayManager>();
//                    DontDestroyOnLoad(eventsGameObject);
                    
//                    _instance.Init();
//                }
//                return _instance;
//            }
//        }

//        PlayerController _playerController;
//        PlayerController playerController
//        {
//            get
//            {
//                if (_playerController == null)
//                    _playerController = Resources.FindObjectsOfTypeAll<PlayerController>().FirstOrDefault();

//                return _playerController;
//            }
//        }

//        AudioTimeSyncController _audioTimeSyncController;
//        public AudioTimeSyncController audioTimeSyncController
//        {
//            get
//            {
//                if (_audioTimeSyncController == null)
//                    _audioTimeSyncController = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();

//                return _audioTimeSyncController;
//            }
//        }

//        public Mode mode { get; set; }

//        public Events eventsManager;
//        public VRController _leftController;
//        public VRController _rightController;
//        private List<Keyframe> _keyframes;
//        private int _keyframeIndex;
//        private float _controllersSmooth;
//        public Vector3 _leftRef;
//        public Vector3 _rightRef;
//        public Quaternion _leftQRef;
//        public Quaternion _rightQRef;
//        public bool ready;
//        private void Init()
//        {
//            mode = Mode.Off;
//            eventsManager = Events.Instance;
//            eventsManager.levelStarted += LevelStarted;
//        }

//        private void OnDestroy()
//        {
//            _leftController.enabled = true;
//            _rightController.enabled = true;
//            eventsManager.levelStarted -= LevelStarted;
//            if (mode == Mode.Record)
//                Save();
//        }
//        private void LevelStarted()
//        {
//            mode = Mode.Playback ;
//            if (mode == Mode.Off)
//                enabled = false;


//            _controllersSmooth = 0f;

//            if (mode == Mode.Playback)
//            {
//                _leftController.enabled = false;
//                _rightController.enabled = false;
//            }
//            _keyframes = new List<Keyframe>();

//            if (mode == Mode.Playback)
//                Load();


//            //if (mode == Mode.Record)
//                //enabled = true;
//        }

//        private void Update()
//        {
//            if (mode == Mode.Playback)
//                PlaybackTick();
//        }
//        private void LateUpdate()
//        {
//            if (mode == Mode.Record)
//                RecordTick();
//        }

//        private void RecordTick()
//        {
//            if (audioTimeSyncController.songTime == 0f)
//                return;
//            Keyframe keyframe = new Keyframe();
//            if (_leftController.transform != null)
//                keyframe._pos1 = _leftController.transform.position; keyframe._rot1 = _leftController.transform.rotation;
//            if (_rightController.transform != null)
//                keyframe._pos2 = _rightController.transform.position; keyframe._rot2 = _rightController.transform.rotation;

//            keyframe._time = audioTimeSyncController.songTime;
//            _keyframes.Add(keyframe);
//        }

//        private void PlaybackTick()
//        {
//            float songTime = audioTimeSyncController.songTime;
//            while (_keyframeIndex < _keyframes.Count - 2 && _keyframes[_keyframeIndex + 1]._time < songTime)
//            {
//                _keyframeIndex++;
//            }
//            Keyframe keyframe = _keyframes[_keyframeIndex];
//            Keyframe keyframe2 = _keyframes[_keyframeIndex + 1];
//            float wtf = (songTime - keyframe._time) / Mathf.Max(1E-06f, keyframe2._time - keyframe._time);
//            float wtf2 = (_controllersSmooth == 0f) ? 1f : (Time.deltaTime * _controllersSmooth);
//            if (_leftController.transform != null)
//            {
//                if (!_leftController.transform.gameObject.activeSelf)
//                    _leftController.transform.gameObject.SetActive(true);
//                Vector3 targetPos = Vector3.Lerp(keyframe._pos1, keyframe2._pos1, wtf);
//                Quaternion targetRot = Quaternion.Lerp(keyframe._rot1, keyframe2._rot1, wtf);
//                SetPositionAndRotation(_leftController.transform, targetPos, targetRot, wtf2);
//            }
//            if (_rightController.transform != null)
//            {
//                if (!_rightController.transform.gameObject.activeSelf)
//                    _rightController.transform.gameObject.SetActive(true);
//                Vector3 targetPos = Vector3.Lerp(keyframe._pos2, keyframe2._pos2, wtf);
//                Quaternion targetRot = Quaternion.Lerp(keyframe._rot2, keyframe2._rot2, wtf);
//                SetPositionAndRotation(_rightController.transform, targetPos, targetRot, wtf2);
//            }
//        }

//        public void SetPositionAndRotation(Transform transf, Vector3 targetPos, Quaternion wtfIsAQuaternion, float t)
//        {
//            Vector3 vector = transf.position;
//            Quaternion quaternion = transf.rotation;
//            vector = Vector3.Lerp(vector, targetPos, t);
//            quaternion = Quaternion.Lerp(quaternion, wtfIsAQuaternion, t);
//            transf.SetPositionAndRotation(vector, quaternion);
//        }


//        private void Load()
//        {
//            BinaryFormatter binaryFormatter = new BinaryFormatter();
//            SavedData savedData = null;

//            FileStream fileStream = null;
//            try
//            {
//                fileStream = File.Open(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/Recording.sia", FileMode.Open);
//                savedData = (SavedData)binaryFormatter.Deserialize(fileStream);
//            }
//            catch
//            {
//                savedData = null;
//            }
//            finally
//            {
//                if (fileStream != null)
//                    fileStream.Close();
//            }
//            if (savedData != null)
//                Logger.Log.Notice("Loaded recording...");

//            if (savedData != null)
//            {
//                _keyframes = new List<Keyframe>(savedData._keyframes.Length);
//                for (int i = 0; i < savedData._keyframes.Length; i++)
//		        {
//			        SavedData.KeyframeSerializable keyframeSerializable = savedData._keyframes[i];
//			        Keyframe keyframe = new Keyframe();
//			        keyframe._pos1 = new Vector3(keyframeSerializable._xPos1, keyframeSerializable._yPos1, keyframeSerializable._zPos1);
//			        keyframe._pos2 = new Vector3(keyframeSerializable._xPos2, keyframeSerializable._yPos2, keyframeSerializable._zPos2);
//			        keyframe._rot1 = new Quaternion(keyframeSerializable._xRot1, keyframeSerializable._yRot1, keyframeSerializable._zRot1, keyframeSerializable._wRot1);
//			        keyframe._rot2 = new Quaternion(keyframeSerializable._xRot2, keyframeSerializable._yRot2, keyframeSerializable._zRot2, keyframeSerializable._wRot2);
//			        keyframe._time = keyframeSerializable._time;
//			        _keyframes.Add(keyframe);
//		        }
//                ready = true;
//		        return;
//            }
//            Logger.Log.Error("Could not deserialize!");
//            enabled = false;
//        }

//        private void Save()
//        {
//            BinaryFormatter binaryFormatter = new BinaryFormatter();
//            FileStream fileStream = File.Open(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/Recording.sia", FileMode.OpenOrCreate);
//            SavedData savedData = new SavedData();
//            savedData._keyframes = new SavedData.KeyframeSerializable[_keyframes.Count];
//            for (int i = 0; i < _keyframes.Count; i++)
//            {
//                Keyframe keyframe = _keyframes[i];
//                SavedData.KeyframeSerializable keyframeSerializable = new SavedData.KeyframeSerializable();
//                keyframeSerializable._xPos1 = keyframe._pos1.x;
//                keyframeSerializable._yPos1 = keyframe._pos1.y;
//                keyframeSerializable._zPos1 = keyframe._pos1.z;
//                keyframeSerializable._xPos2 = keyframe._pos2.x;
//                keyframeSerializable._yPos2 = keyframe._pos2.y;
//                keyframeSerializable._zPos2 = keyframe._pos2.z;
//                keyframeSerializable._xRot1 = keyframe._rot1.x;
//                keyframeSerializable._yRot1 = keyframe._rot1.y;
//                keyframeSerializable._zRot1 = keyframe._rot1.z;
//                keyframeSerializable._wRot1 = keyframe._rot1.w;
//                keyframeSerializable._xRot2 = keyframe._rot2.x;
//                keyframeSerializable._yRot2 = keyframe._rot2.y;
//                keyframeSerializable._zRot2 = keyframe._rot2.z;
//                keyframeSerializable._wRot2 = keyframe._rot2.w;
//                keyframeSerializable._time = keyframe._time;
//                savedData._keyframes[i] = keyframeSerializable;
//            }
//            binaryFormatter.Serialize(fileStream, savedData);
//            fileStream.Close();
//            Logger.Log.Notice("Data saved to:" + Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/Replays/Recording.sia");
//        }


//        private class Keyframe
//        {
//            public Vector3 _pos1;
//            public Vector3 _pos2;
//            public Quaternion _rot1;
//            public Quaternion _rot2;
//            public float _time;
//        }

//        [Serializable]
//        private class SavedData
//        {
//            public KeyframeSerializable[] _keyframes;
//            [Serializable]
//            public class KeyframeSerializable
//            {
//                public float _xPos1;
//                public float _yPos1;
//                public float _zPos1;
//                public float _xPos2;
//                public float _yPos2;
//                public float _zPos2;
//                public float _xRot1;
//                public float _yRot1;
//                public float _zRot1;
//                public float _wRot1;
//                public float _xRot2;
//                public float _yRot2;
//                public float _zRot2;
//                public float _wRot2;
//                public float _time;
//            }
//        }

//        public enum Mode
//        {
//            Playback,
//            Record,
//            Off
//        }
//    }

//}

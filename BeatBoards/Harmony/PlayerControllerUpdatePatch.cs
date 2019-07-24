using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPA.Utilities;
using BeatBoards.Replays;
using BeatBoards.Utilities;
using UnityEngine;
using Logger = BeatBoards.Utilities.Logger;

namespace BeatBoards.Harmony
{
    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("Update")]
    class PlayerControllerUpdatePatch
    {
        static void Postfix(ref Saber ____leftSaber, ref Saber ____rightSaber)
        {
            if (ReplayManager.Instance.playback == true && ReplayManager.Instance.gameObjectActive == true)
            {
                float songTime = (float)Math.Round(ReplayManager.Instance.audioTimeSyncController.songTime, 2);

                PositionData posDat = null;
                bool axe = ReplayManager.Instance.posDictionary.TryGetValue(songTime, out posDat);

                if (posDat != null && axe == true)
                {
                    ____leftSaber.transform.position = new Vector3(posDat.LeftSaber.PositionX, posDat.LeftSaber.PositionY, posDat.LeftSaber.PositionZ);
                    ____rightSaber.transform.position = new Vector3(posDat.RightSaber.PositionX, posDat.RightSaber.PositionY, posDat.RightSaber.PositionZ);
                    ____leftSaber.transform.rotation = Quaternion.Euler(posDat.LeftSaber.RotationX, posDat.LeftSaber.RotationY, posDat.LeftSaber.RotationZ);
                    ____rightSaber.transform.rotation = Quaternion.Euler(posDat.RightSaber.RotationX, posDat.RightSaber.RotationY, posDat.RightSaber.RotationZ);
                }
            }
            //{
            //    float songTime = ReplayManager.Instance.audioTimeSyncController.songTime;

            //    PositionData posDat = null;
            //    bool axe = ReplayManager.Instance.posDictionary.TryGetValue(songTime, out posDat);

            //    if (axe == true)
            //    {
            //        Logger.Log.Warn("Okay, this is epic.");
            //    }
            //}

            //if (ReplayManager.Instance.playback == true)
            //{
            //    float songTime = ReplayManager.Instance.audioTimeSyncController.songTime;

            //    PositionData positionData = ReplayManager.Instance.playBackData.Where(i => i.SongTime == songTime).First();

            //    if (positionData != null || positionData != new PositionData() { })
            //    {
            //        playerController.leftSaber.transform.position = new Vector3(positionData.LeftSaber.PositionX, positionData.LeftSaber.PositionY, positionData.LeftSaber.PositionZ);
            //        playerController.rightSaber.transform.position = new Vector3(positionData.RightSaber.PositionX, positionData.RightSaber.PositionY, positionData.RightSaber.PositionZ);
            //        playerController.leftSaber.transform.rotation = Quaternion.Euler(positionData.LeftSaber.RotationX, positionData.LeftSaber.RotationY, positionData.LeftSaber.RotationZ);
            //        playerController.rightSaber.transform.rotation = Quaternion.Euler(positionData.RightSaber.RotationX, positionData.RightSaber.RotationY, positionData.RightSaber.RotationZ);
            //    }
            //}
        }
    }
}

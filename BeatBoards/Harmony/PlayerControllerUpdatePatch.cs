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
        static void Postfix(ref Saber ____leftSaber, ref Saber ____rightSaber, ref Transform ____headTransform)
        {

            if (ReplayManager.Instance.recording == true && ReplayManager.Instance.gameObjectActive == true)
            {
                Vector3 leftSaberPos = ____leftSaber.transform.position;
                Vector3 leftSaberRot = ____leftSaber.transform.rotation.eulerAngles;
                Vector3 rightSaberPos = ____rightSaber.transform.position;
                Vector3 rightSaberRot = ____rightSaber.transform.rotation.eulerAngles;
                Vector3 headPos = ____headTransform.position;
                Vector3 headRot = ____headTransform.rotation.eulerAngles;

                ReplayManager.Instance.positionData.Add(new PositionData()
                {
                    SongTime = ReplayManager.Instance.audioTimeSyncController.songTime,
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
                    },
                    Head = new HeadData()
                    {
                        PositionX = headPos.x,
                        PositionY = headPos.y,
                        PositionZ = headPos.z,
                        RotationX = headRot.x,
                        RotationY = headRot.y,
                        RotationZ = headRot.z
                    }
                });
            }

            if (ReplayManager.Instance.playback == true && ReplayManager.Instance.gameObjectActive == true)
            {
                float songTime = ReplayManager.Instance.audioTimeSyncController.songTime;

                PositionData posDat = null;
                //bool axe = ReplayManager.Instance.posDictionary.TryGetValue(songTime, out posDat);

                int index = Conversions.FindClosestIndex(ReplayManager.Instance.playBackData, songTime);
                index = Math.Max(index, 0);
                //posDat = ReplayManager.Instance.playBackData.OrderBy(x => Math.Abs(songTime - x.SongTime)).ThenByDescending(x => x).First();
                //Logger.Log.Info(posDat.LeftSaber.PositionX.ToString());
                posDat = ReplayManager.Instance.playBackData[index];


                if (posDat != null)
                {
                    ____leftSaber.transform.position = new Vector3(posDat.LeftSaber.PositionX, posDat.LeftSaber.PositionY, posDat.LeftSaber.PositionZ);
                    ____rightSaber.transform.position = new Vector3(posDat.RightSaber.PositionX, posDat.RightSaber.PositionY, posDat.RightSaber.PositionZ);
                    ____leftSaber.transform.rotation = Quaternion.Euler(posDat.LeftSaber.RotationX, posDat.LeftSaber.RotationY, posDat.LeftSaber.RotationZ);
                    ____rightSaber.transform.rotation = Quaternion.Euler(posDat.RightSaber.RotationX, posDat.RightSaber.RotationY, posDat.RightSaber.RotationZ);
                    //____headTransform.rotation = Quaternion.Euler(posDat.Head.RotationX, posDat.Head.RotationY, posDat.Head.RotationZ);
                    //____headTransform.rotation = Quaternion.Euler(posDat.Head.RotationX, posDat.Head.RotationY, posDat.Head.RotationZ);
                }
            }
        }
    }
}

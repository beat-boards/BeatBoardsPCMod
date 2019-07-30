//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Harmony;
//using UnityEngine;
//using BeatBoards.Utilities;
//using Logger = BeatBoards.Utilities.Logger;
//using UnityEngine.XR;

//namespace BeatBoards.Harmony
//{
//    [HarmonyPatch(typeof(VRController))]
//    [HarmonyPatch("UpdatePositionAndRotation")]
//    class VRControllerPatch
//    {
//        static void Postfix(ref Vector3 ____lastTrackedPosition, ref VRController __instance, ref XRNode ____node)
//        {
//            if (____node == XRNode.LeftHand) Replays.ReplayManager.Instance._leftController = __instance;
//            if (____node == XRNode.RightHand) Replays.ReplayManager.Instance._rightController = __instance;

//            //if (Replays.ReplayManager.Instance.ready == true)
//            //{
//            //    if (____node == XRNode.LeftHand)
//            //        __instance.transform.position = Replays.ReplayManager.Instance._leftRef; __instance.transform.rotation = Replays.ReplayManager.Instance._leftQRef;
//            //    if (____node == XRNode.RightHand)
//            //        __instance.transform.position = Replays.ReplayManager.Instance._rightRef; __instance.transform.rotation = Replays.ReplayManager.Instance._rightQRef;
//            //    return false;
//            //}
            
//           // return true;
//        }
//    }
//}

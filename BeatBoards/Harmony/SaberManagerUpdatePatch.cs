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
    [HarmonyPatch(typeof(Saber))]
    [HarmonyPatch("ManualUpdate")]

    public class SaberManagerUpdatePatch
    {
        static bool Prefix()
        {
            if (ReplayManager.Instance.gameObjectActive == true && ReplayManager.Instance.playback)
            {
                return false;
            }
            return false;
        }
    }
}

using BeatBoards.Utilities;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace BeatBoards.Harmony
{
    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("RefreshContent")]

    public class StandardLevelDetailViewContentPresented
    {
        static void Postfix(ref IDifficultyBeatmap ____selectedDifficultyBeatmap)
        {
            //Logger.Log.Critical(____selectedDifficultyBeatmap.level.songName);
        }
    }
}

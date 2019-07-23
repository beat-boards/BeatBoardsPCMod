using BeatBoards.Utilities;
using BeatBoards.Core;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBoards.Harmony
{
    [HarmonyPatch(typeof(PlatformLeaderboardViewController))]
    [HarmonyPatch("Refresh")]
    class PlatformLeaderboardPatch
    {
        static bool Prefix(ref IDifficultyBeatmap ____difficultyBeatmap, ref List<LeaderboardTableView.ScoreData> ____scores, ref bool ____hasScoresData, ref LeaderboardTableView ____leaderboardTableView, ref int[] ____playerScorePos, ref PlatformLeaderboardsModel.ScoresScope ____scoresScope)
        {
            if (____difficultyBeatmap.level is CustomBeatmapLevel)
            {
                ____hasScoresData = false;
                ____scores.Clear();
                ____leaderboardTableView.SetScores(____scores, ____playerScorePos[(int)____scoresScope]);
                Logger.Log.Warn("CustomBeatmap");
                Events.Instance.leaderboardOpened.Invoke(____difficultyBeatmap, ____leaderboardTableView);
                return false;

            }
            Logger.Log.Warn("Base game song");
            return true;
        }
    }
}

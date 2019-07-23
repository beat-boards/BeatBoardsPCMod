using BeatBoards.Utilities;
using BeatBoards.Core;
using Harmony;
using System.Collections.Generic;
using HMUI;
using IPA.Utilities;
using System.Linq;
using BeatBoards.UI;

namespace BeatBoards.Harmony
{
    [HarmonyPatch(typeof(PlatformLeaderboardViewController))]
    [HarmonyPatch("Refresh")]
    class PlatformLeaderboardPatch
    {

        static bool Prefix(ref IDifficultyBeatmap ____difficultyBeatmap, ref List<LeaderboardTableView.ScoreData> ____scores, ref bool ____hasScoresData, ref LeaderboardTableView ____leaderboardTableView, ref int[] ____playerScorePos, ref PlatformLeaderboardsModel.ScoresScope ____scoresScope, ref IconSegmentedControl ____scopeSegmentedControl)
        {
            if (____difficultyBeatmap.level is CustomBeatmapLevel)
            {
                IconSegmentedControl.DataItem thirdCell = ____scopeSegmentedControl.GetPrivateField<IconSegmentedControl.DataItem[]>("_dataItems").Last();
                thirdCell.SetPrivateProperty("hintText", "Platform: PC");
                thirdCell.SetPrivateProperty("icon", LeaderboardUIManager.Instance.PCIcon);
                //____scopeSegmentedControl.ReloadData();

                ____hasScoresData = false;
                ____scores.Clear();
                ____leaderboardTableView.SetScores(____scores, ____playerScorePos[(int)____scoresScope]);
                Events.Instance.leaderboardOpened.Invoke(____difficultyBeatmap, ____leaderboardTableView);
                return false;

            }
            return true;
        }
    }
}

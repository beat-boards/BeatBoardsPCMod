using Harmony;

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

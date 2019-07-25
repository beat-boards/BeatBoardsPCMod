using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatBoards.UI;
using CustomUI.BeatSaber;
using Harmony;
using HMUI;
using UnityEngine;

namespace BeatBoards.Harmony
{
    [HarmonyPatch(typeof(PlatformLeaderboardViewController))]
    [HarmonyPatch("DidActivate")]
    class PlatformLeaderboardActivation
    {
        static void Postfix(ref Sprite ____friendsLeaderboardIcon, ref Sprite ____globalLeaderboardIcon, ref Sprite ____aroundPlayerLeaderboardIcon, ref IconSegmentedControl ____scopeSegmentedControl, ref PlatformLeaderboardViewController __instance)
        {//LeaderboardUIManager.Instance.PCIcon;
            ____scopeSegmentedControl.didSelectCellEvent += __instance.HandleScopeSegmentedControlDidSelectCell;

            ____scopeSegmentedControl.SetData(new IconSegmentedControl.DataItem[] {
                new IconSegmentedControl.DataItem(____globalLeaderboardIcon, "Global"),
                new IconSegmentedControl.DataItem(____aroundPlayerLeaderboardIcon, "Around You"),
                new IconSegmentedControl.DataItem(LeaderboardUIManager.Instance.PCIcon, "Platform: PC")
            });

            var replayButton = __instance.CreateUIButton("OkButton", new Vector2(0f, -37f), new Vector2(9.0f, 5.0f));
            replayButton.ToggleWordWrapping(false);
            replayButton.SetButtonText("Replays");
            replayButton.SetButtonTextSize(3);
            replayButton.onClick.AddListener(LeaderboardUIManager.Instance.ReplayMenu);

            LeaderboardUIManager.Instance.replaysButton = replayButton;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CustomUI.BeatSaber;
using TMPro;
using BeatBoards.Utilities;
using UnityEngine.UI;
using Logger = BeatBoards.Utilities.Logger;

namespace BeatBoards.UI
{
    public class BeatBoardsMenu : MonoBehaviour
    {
        public static void Load() { new GameObject("BeatBoards: Menu Manager").AddComponent<BeatBoardsMenu>(); }

        public TextMeshProUGUI loadingText;
        public CustomMenu beatBoardsMenu;
        public CustomViewController mainViewController;

        private TextMeshProUGUI _identifierText;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _accountStatus;

        private TextMeshProUGUI _rankPointsText;
        private TextMeshProUGUI _rankText;
        private TextMeshProUGUI _roleText;

        private Sprite _userID;

        private void Awake()
        {

        }

        private void Start()
        {
            beatBoardsMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Beat BoardS");
            mainViewController = BeatSaberUI.CreateViewController<CustomViewController>();
            loadingText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(0, 0));
            loadingText.alignment = TextAlignmentOptions.Center;

            _identifierText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, 30));
            _identifierText.alignment = TextAlignmentOptions.BaselineLeft;
            _nameText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, 25));
            _nameText.alignment = TextAlignmentOptions.BaselineLeft;
            _accountStatus = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, 20));
            _accountStatus.alignment = TextAlignmentOptions.BaselineLeft;
            _rankPointsText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, -20));
            _rankPointsText.alignment = TextAlignmentOptions.BaselineLeft;
            _rankText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, -25));
            _rankText.alignment = TextAlignmentOptions.BaselineLeft;
            _roleText = BeatSaberUI.CreateText(mainViewController.rectTransform, "", new Vector2(-35, -30));
            _roleText.alignment = TextAlignmentOptions.BaselineLeft;

            PresentData();
        }

        private async void GetData()
        {
            loadingText.SetText("Fetching Data...");

            bool fakeRequest = true;

            if (fakeRequest == true)
            {
                loadingText.SetText("");
                PresentData();
            }
        }

        private void PresentData()
        {
            Logger.Log.Info("Test");
            beatBoardsMenu.SetMainViewController(mainViewController, true, (firstActivation, type) =>
            {
                _identifierText.SetText("<b>User ID:</b> " + "76561198088728803");
                _nameText.SetText("<b>Name:</b> " + "Auros™");
                _accountStatus.SetText("<b>Status:</b> " + "Active");
                _rankPointsText.SetText("<b>Rank Points:</b> " + "2043");
                _rankText.SetText("<b>Global Rank:</b> " + "23");
                _roleText.SetText("<b>Role:</b> " + "<color=#00ffff>Owner</color>");

                //UtilityManager.Instance.CreateImageFromURL("https://avatars0.githubusercontent.com/u/41306347");



                var _userImage = new GameObject("BeatBoards: User Image").AddComponent<Image>();
                _userImage.material = CustomUI.Utilities.UIUtilities.NoGlowMaterial;
                _userImage.rectTransform.sizeDelta = new Vector2(28f, 28f);
                _userImage.rectTransform.SetParent(mainViewController.transform, false);

                SharedCoroutineStarter.instance.StartCoroutine(UtilityManager.LoadScripts.LoadSpriteCoroutine("https://avatars0.githubusercontent.com/u/41306347", (image) =>
                {
                    _userImage.sprite = image;
                }));

                _userImage.transform.localPosition = new Vector3(-51, 1);

                Logger.Log.Info("Test 2");
            });
            beatBoardsMenu.Present();
        }

        public void Cleanup()
        {

        }
    }
}

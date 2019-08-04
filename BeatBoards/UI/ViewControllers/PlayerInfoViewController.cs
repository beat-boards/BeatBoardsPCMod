using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.BeatSaber;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRUI;

namespace BeatBoards.UI.ViewControllers
{
    class PlayerInfoViewController : CustomViewController
    {
        private GameObject _loadingIndicator;

        public TextMeshProUGUI loadingText;

        private TextMeshProUGUI _identifierText;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _accountStatus;

        private TextMeshProUGUI _rankPointsText;
        private TextMeshProUGUI _rankText;
        private TextMeshProUGUI _roleText;

        private TextMeshProUGUI _randomText;
        public TextMeshProUGUI _titleText;

        public Button EditNameButton;
        public Button EditProfileButton;

        public Action<string> NameButtonPressed;
        public Action ImageButtonPressed;

        public override void __Activate(ActivationType activationType)
        {
            base.__Activate(activationType);

            if (activationType == ActivationType.AddedToHierarchy)
            {
                _loadingIndicator = BeatSaberUI.CreateLoadingSpinner(rectTransform);
                (_loadingIndicator.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
                (_loadingIndicator.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
                (_loadingIndicator.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f);
                _loadingIndicator.SetActive(true);

                RectTransform viewControllersContainer = FindObjectsOfType<RectTransform>().First(x => x.name == "ViewControllers");
                var headerPanelRectTransform = Instantiate(viewControllersContainer.GetComponentsInChildren<RectTransform>(true).First(x => x.name == "HeaderPanel" && x.parent.name == "PlayerSettingsViewController"), rectTransform);
                headerPanelRectTransform.name = "BeatBoards: Header";
                headerPanelRectTransform.gameObject.SetActive(true);
                _titleText = headerPanelRectTransform.GetComponentInChildren<TextMeshProUGUI>();
                _titleText.text = "";

                
                CreateText();
            }
        }

        

        private void CreateText()
        {
            if (loadingText == null && _identifierText == null && _nameText == null && _accountStatus == null && _rankPointsText == null && _rankText == null && _roleText == null && _randomText == null)
            {
                loadingText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(15, 0));
                loadingText.alignment = TextAlignmentOptions.Center;

                _identifierText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, 26));
                _identifierText.alignment = TextAlignmentOptions.BaselineLeft;
                _nameText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, 20));
                _nameText.alignment = TextAlignmentOptions.BaselineLeft;
                _accountStatus = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, 14));
                _accountStatus.alignment = TextAlignmentOptions.BaselineLeft;
                _rankPointsText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, 8));
                _rankPointsText.alignment = TextAlignmentOptions.BaselineLeft;
                _rankText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, 2));
                _rankText.alignment = TextAlignmentOptions.BaselineLeft;
                _roleText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(27, -4));
                _roleText.alignment = TextAlignmentOptions.BaselineLeft;

                _randomText = BeatSaberUI.CreateText(rectTransform, "", new Vector2(0, -23));
                _randomText.alignment = TextAlignmentOptions.Center;
            }

            _loadingIndicator.SetActive(false);
            FakeData();
            CreateButtons();
        }

        private void CreateButtons()
        {
            if (EditNameButton == null)
            {
                EditNameButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(-55, 25));
                EditNameButton.ToggleWordWrapping(false);
                EditNameButton.SetButtonText("Edit Name");
                EditNameButton.SetButtonTextSize(4);
                EditNameButton.onClick.AddListener(delegate { NameButtonPressed.Invoke(_nameText.text); });
            }
            
            if (EditProfileButton == null)
            {
                EditProfileButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(-55, 15));
                EditProfileButton.ToggleWordWrapping(false);
                EditProfileButton.SetButtonText("Change Image");
                EditProfileButton.SetButtonTextSize(3);
                EditProfileButton.onClick.AddListener(delegate { ImageButtonPressed.Invoke(); });
            }
        }

        private void FakeData()
        {
            RawImage rawImage = Image(profilepictureb64);
            rawImage.transform.localPosition = new Vector3(100f, 12f);
            _titleText.text = "Auros";

            StartCoroutine(MoveProfileAnimation(rawImage));
        }

        private IEnumerator MoveProfileAnimation(RawImage rawImage)
        {
            RawImage profilePicture = rawImage;
            while (profilePicture.transform.localPosition.x > -22)
            {

                profilePicture.transform.localPosition = new Vector3((profilePicture.transform.localPosition.x - 4), 12f);

                yield return new WaitForSecondsRealtime(.01f);
            }
            SetFakeData();
            yield break;
        }

        private void SetFakeData()
        {
            _identifierText.text = "<i><color=#a1a1a1>UUID: ae27ab86-2e8d-4008-a1d2-05e5d4ff9334</color></i>";
            _nameText.text = "Name: Auros";
            _accountStatus.text = "Status: <color=green>Active</color>";
            _rankPointsText.text = "Ranking Points: 2451.19";
            _rankText.text = "Rank: 1";
            _roleText.text = "Role: <color=#00ffff>Owner</color>";

            _randomText.text = "\"Sometimes, it really do be like that\"\n- Guy 2018";
        }

        public RawImage Image(string imageb64, float size = 36)
        {
            var _userImage = new GameObject("BeatBoards: Image").AddComponent<RawImage>();
            _userImage.material = CustomUI.Utilities.UIUtilities.NoGlowMaterial;
            _userImage.rectTransform.sizeDelta = new Vector2(size, size);
            _userImage.rectTransform.SetParent(rectTransform.transform, false);
            Texture2D tex = new Texture2D(1, 1);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadImage(Convert.FromBase64String(imageb64));
            _userImage.texture = tex;

            return _userImage;
        }


        public const string profilepictureb64 = "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";
    }
}

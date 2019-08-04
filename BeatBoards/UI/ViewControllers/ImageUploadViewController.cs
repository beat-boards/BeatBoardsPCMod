using System;
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
    class ImageUploadViewController : CustomViewController
    {
        public TextMeshProUGUI _statusText;
        private Button _refreshButton;
        public Button _setButton;
        private Button _closeButton;
        public TextMeshProUGUI _titleText;

        public Action refreshButtonClicked;
        public Action setButtonClicked;
        public Action closeButtonClicked;
        
        public override void __Activate(ActivationType activationType)
        {
            base.__Activate(activationType);

            if (activationType == ActivationType.AddedToHierarchy)
            {
                if (_titleText == null)
                {
                    RectTransform viewControllersContainer = FindObjectsOfType<RectTransform>().First(x => x.name == "ViewControllers");
                    var headerPanelRectTransform = Instantiate(viewControllersContainer.GetComponentsInChildren<RectTransform>(true).First(x => x.name == "HeaderPanel" && x.parent.name == "PlayerSettingsViewController"), rectTransform);
                    headerPanelRectTransform.name = "BeatBoards: Header";
                    headerPanelRectTransform.gameObject.SetActive(true);
                    _titleText = headerPanelRectTransform.GetComponentInChildren<TextMeshProUGUI>();
                    _titleText.text = "Change Profile Picture";
                }

                _statusText = BeatSaberUI.CreateText(rectTransform, "Place a .jpg or .png in <u>UserData/BeatBoards/ProfilePicture</u>\nMaximum file size: <color=red>100KB</color>\nNo File Found.", new Vector2(0, 0));
                _statusText.alignment = TextAlignmentOptions.Center;

                _refreshButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(0, -30));
                _setButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(20, -30));
                _closeButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(-20, -30));
                _refreshButton.SetButtonText("Refresh");
                _setButton.SetButtonText("Set");
                _closeButton.SetButtonText("Close");
                _refreshButton.ToggleWordWrapping(false);
                _setButton.ToggleWordWrapping(false);
                _closeButton.ToggleWordWrapping(false);
                _refreshButton.onClick.AddListener(delegate { refreshButtonClicked.Invoke(); });
                _setButton.onClick.AddListener(delegate { setButtonClicked.Invoke(); });
                _closeButton.onClick.AddListener(delegate { closeButtonClicked.Invoke(); });

                _setButton.interactable = false;
            }
        }
    }
}

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
        private TextMeshProUGUI _statusText;
        private Button _refreshButton;
        private Button _setButton;
        private Button _closeButton;

        public Action refreshButtonClicked;
        public Action setButtonClicked;
        public Action closeButtonClicked;
        
        public override void __Activate(ActivationType activationType)
        {
            base.__Activate(activationType);

            if (activationType == ActivationType.AddedToHierarchy)
            {
                _statusText = BeatSaberUI.CreateText(rectTransform, "Place a .jpg or .png in <u>UserData/BeatBoards/ProfilePicture</u>\nMaximum file size: <color=red>2MB</color>", new Vector2(0, 0));
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

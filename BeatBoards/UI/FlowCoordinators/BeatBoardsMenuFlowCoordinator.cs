using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BeatBoards.UI.ViewControllers;
using CustomUI.BeatSaber;
using CustomUI.Utilities;
using HMUI;
using UnityEngine;
using UnityEngine.UI;
using BeatBoards.UI.ViewControllers;
using VRUI;
using System.IO;

namespace BeatBoards.UI.FlowCoordinators
{
    public class BeatBoardsMenuFlowCoordinator : FlowCoordinator
    {
        private BackButtonNavigationController _beatBoardsMenuNavigationController;
        private PlayerInfoViewController _playerInfoViewController;
        private FriendsListViewController _friendsListViewController;
        private KeyboardViewController _keyboardViewController;
        private KeyboardViewController _addKeyboardViewController;
        private ImageUploadViewController _imageUploadViewController;

        public void Awake()
        {
            if (_beatBoardsMenuNavigationController == null)
            {
                _beatBoardsMenuNavigationController = BeatSaberUI.CreateViewController<BackButtonNavigationController>();
                _beatBoardsMenuNavigationController.didFinishEvent += _beatBoardsNavigationController_didFinishEvent;
            }
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation && activationType == ActivationType.AddedToHierarchy)
            {
                title = "Beat Boards Menu";

                _playerInfoViewController = BeatSaberUI.CreateViewController<PlayerInfoViewController>();
                _friendsListViewController = BeatSaberUI.CreateViewController<FriendsListViewController>();

                _playerInfoViewController.NameButtonPressed += _playerInfoViewController_editNameButtonPressed;
                _playerInfoViewController.ImageButtonPressed += _playerInfoViewController_editImageButtonPressed;

                _friendsListViewController.AddFriendButtonPressed += _friendsListViewController_addFriendButtonPressed;
            }

            SetViewControllersToNavigationConctroller(_beatBoardsMenuNavigationController, new VRUIViewController[]
            {
                _playerInfoViewController
            });
            ProvideInitialViewControllers(_beatBoardsMenuNavigationController, _friendsListViewController);
        }

        private void _friendsListViewController_addFriendButtonPressed()
        {
            if (_addKeyboardViewController == null)
            {
                _addKeyboardViewController = BeatSaberUI.CreateViewController<KeyboardViewController>();
                _addKeyboardViewController.searchButtonPressed += _addFriendEnterPressed;
            }
            PresentViewController(_addKeyboardViewController);
        }

        private void _addFriendEnterPressed(string obj)
        {
            DismissViewController(_addKeyboardViewController);
        }

        private void _playerInfoViewController_editImageButtonPressed()
        {
            if (_imageUploadViewController == null)
            {
                _imageUploadViewController = BeatSaberUI.CreateViewController<ImageUploadViewController>();
                _imageUploadViewController.refreshButtonClicked += _imageUploadViewController_refreshButtonPressed;
                _imageUploadViewController.setButtonClicked += _imageUploadViewController_setButtonPressed;
                _imageUploadViewController.closeButtonClicked += _imageUploadViewController_backButtonPressed;
            }

            if (_imageUploadViewController._titleText != null)
                _imageUploadViewController._titleText.text = "Change Profile Picture";

            PresentViewController(_imageUploadViewController);
        }

        private void _imageUploadViewController_refreshButtonPressed()
        {
            DestroyUploadImages();
            if (Directory.Exists(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/BeatBoards/ProfilePicture"))
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory.Replace('\\', '/') + "/UserData/BeatBoards/ProfilePicture");
                _imageUploadViewController._statusText.rectTransform.localPosition = new Vector3(0, 0);
                if (files.Length == 1)
                {
                    var image = files.First();

                    if (image.EndsWith(".png") || image.EndsWith(".jpg"))
                    {
                        Utilities.Logger.Log.Info(image);

                        Texture2D texture = null;

                        FileInfo fileInfo = new FileInfo(image);
                        long file = fileInfo.Length;

                        Utilities.Logger.Log.Info(_imageUploadViewController._statusText.rectTransform.localPosition.ToString());

                        if (file > 110000)
                        {
                            _imageUploadViewController._statusText.text = "Image is <color=red>TOO BIG</color>!\nMaximum file size is <color=#00ffff>100KB</color>";
                            _imageUploadViewController._setButton.interactable = false;
                            return;
                        }

                        _imageUploadViewController._statusText.rectTransform.localPosition = new Vector3(0, 20); 
                        texture = UIUtilities.LoadTextureFromFile(image);
                        _imageUploadViewController._statusText.text = $"<color=green>{fileInfo.Name}</color>";
                        _imageUploadViewController._setButton.interactable = true;

                        if (texture == null)
                        {
                            DestroyUploadImages();

                            _imageUploadViewController._statusText.rectTransform.localPosition = new Vector3(0, 0);
                            _imageUploadViewController._statusText.text = "Image Invalid!";
                            _imageUploadViewController._setButton.interactable = false;
                            return;
                        }

                        DestroyUploadImages();
                        Image(texture, _imageUploadViewController);
                    }
                }
                else if (files.Length > 1)
                {
                    _imageUploadViewController._statusText.text = "Multiple Files Detected! Please only have 1 file in the folder!";
                    _imageUploadViewController._setButton.interactable = false;
                }
                else if (files.Length == 0)
                {
                    _imageUploadViewController._statusText.text = "No Files Detected!";
                    _imageUploadViewController._setButton.interactable = false;
                }
                else
                {
                    _imageUploadViewController._statusText.text = "You... shouldn't be seeing this. What did you do?\nWell, I hope you're having a good day. <color=yellow>Try again</color>.";
                    _imageUploadViewController._setButton.interactable = false;
                }
            }
        }

        private void _imageUploadViewController_setButtonPressed()
        {
            
            DestroyUploadImages();
            DismissViewController(_imageUploadViewController);
        }

        private void _imageUploadViewController_backButtonPressed()
        {
            DestroyUploadImages();
            DismissViewController(_imageUploadViewController);
        }

        private void _playerInfoViewController_editNameButtonPressed(string text)
        {
            if (_keyboardViewController == null)
            {
                _keyboardViewController = BeatSaberUI.CreateViewController<KeyboardViewController>();
                
                _keyboardViewController.backButtonPressed += _keyboardViewController_backButtonPressed;
                _keyboardViewController.searchButtonPressed += _keyboardViewController_searchButtonPressed;
            }

            PresentViewController(_keyboardViewController);
        }

        private void _keyboardViewController_searchButtonPressed(string obj)
        {
            DismissViewController(_keyboardViewController);
        }

        private void _keyboardViewController_backButtonPressed()
        {
            DismissViewController(_keyboardViewController);
        }

        private void _beatBoardsNavigationController_didFinishEvent()
        {
            MainFlowCoordinator mainFlow = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().First();
            mainFlow.InvokeMethod("DismissFlowCoordinator", this, null, false);

            var images = FindObjectsOfType<GameObject>().Where(x => x.name == "BeatBoards: Image");
            var headers = FindObjectsOfType<RectTransform>().Where(x => x.name == "BeatBoards: Header");

            foreach (var image in images)
            {
                Destroy(image);
            }

            foreach (var header in headers)
            {
                Destroy(header.gameObject);
            }
        }

        private void DestroyUploadImages()
        {
            var images = FindObjectsOfType<GameObject>().Where(x => x.name == "BeatBoards: Upload Image");
            foreach (var image in images)
            {
                Destroy(image);
            }
        }

        RawImage Image(Texture2D texture, ImageUploadViewController viewController, float size = 36)
        {
            var _userImage = new GameObject("BeatBoards: Upload Image").AddComponent<RawImage>();
            _userImage.material = UIUtilities.NoGlowMaterial;
            _userImage.rectTransform.sizeDelta = new Vector2(size, size);
            _userImage.rectTransform.SetParent(viewController.rectTransform.transform, false);
            Texture2D tex = texture;
            tex.wrapMode = TextureWrapMode.Clamp;
            _userImage.texture = tex;

            return _userImage;
        }
    }
}

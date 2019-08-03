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

namespace BeatBoards.UI.FlowCoordinators
{
    public class BeatBoardsMenuFlowCoordinator : FlowCoordinator
    {
        private BackButtonNavigationController _beatBoardsMenuNavigationController;
        private PlayerInfoViewController _playerInfoViewController;
        private FriendsListViewController _friendsListViewController;
        private KeyboardViewController _keyboardViewController;
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
            }

            SetViewControllersToNavigationConctroller(_beatBoardsMenuNavigationController, new VRUIViewController[]
            {
                _playerInfoViewController
            });
            ProvideInitialViewControllers(_beatBoardsMenuNavigationController, _friendsListViewController);
        }

        private void _playerInfoViewController_editImageButtonPressed()
        {
            if (_imageUploadViewController == null)
            {
                _imageUploadViewController = BeatSaberUI.CreateViewController<ImageUploadViewController>();
                _imageUploadViewController.setButtonClicked += _imageUploadViewController_setButtonPressed;
                _imageUploadViewController.closeButtonClicked += _imageUploadViewController_backButtonPressed;
            }

            PresentViewController(_imageUploadViewController);
        }

        private void _imageUploadViewController_setButtonPressed()
        {
            DismissViewController(_imageUploadViewController);
        }

        private void _imageUploadViewController_backButtonPressed()
        {
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
    }
}

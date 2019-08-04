using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRUI;
using CustomUI.BeatSaber;
using BeatBoards.Core;
using HMUI;
using UnityEngine.UI;

namespace BeatBoards.UI.ViewControllers
{
    class FriendsListViewController : CustomListViewController
    {
        public List<Following> Followers = new List<Following>();

        private int _lastSelectedRow;

        public event Action PageUpPressed;
        public event Action PageDownPressed;

        public Button RemoveFollowerButton;
        public Button AddFollowerButton;

        public Action AddFriendButtonPressed;

        public override void __Activate(ActivationType activationType)
        {
            base.__Activate(activationType);
            
            if (activationType == ActivationType.AddedToHierarchy)
            {
                //(_pageUpButton.transform as RectTransform).anchoredPosition = new Vector2(0, 20);
                _pageUpButton.onClick.AddListener(delegate ()
                {
                    PageUpPressed?.Invoke();
                });
                _pageDownButton.onClick.AddListener(delegate ()
                {
                    PageDownPressed?.Invoke();
                });

                _customListTableView.didSelectCellWithIdxEvent += FriendSelected;

                CreateButtons();

                Followers.Clear();
                Data.Clear();
                SetContent(new List<Following>() { new Following() { Uuid = "cc0d001a-9441-4768-a5e8-56f0e2e612a4", Country = "CA", Fails = 45, Rank = 2, RankingPoints = 910.42f, Role = Role.Owner, Username = "raftario" } });
                
            }
            else
            {
                _customListTableView.ReloadData();
            }
        }

        private void FriendSelected(TableView arg1, int arg2)
        {
            
            _lastSelectedRow = arg2;
            RemoveFollowerButton.interactable = true;
        }

        public void CreateButtons()
        {
            AddFollowerButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(43, 30));
            AddFollowerButton.ToggleWordWrapping(false);
            AddFollowerButton.SetButtonText("Add");
            AddFollowerButton.onClick.AddListener(delegate { AddFriendButtonPressed.Invoke(); });

            RemoveFollowerButton = BeatSaberUI.CreateUIButton(rectTransform, "OkButton", new Vector2(43, 20));
            RemoveFollowerButton.ToggleWordWrapping(false);
            RemoveFollowerButton.SetButtonText("Delete");
            RemoveFollowerButton.interactable = false;
            RemoveFollowerButton.onClick.AddListener(delegate { RemoveFollower(_lastSelectedRow); });
        }

        internal void Refresh()
        {
            _customListTableView.ReloadData();
        }

        public void SetContent(List<Following> followers)
        {
            if (followers == null && Followers != null)
                Followers.Clear();
            else
                Followers = new List<Following>(followers);

            foreach (var follower in Followers)
            {
                Data.Add(new CustomCellInfo(follower.Username, $"Rank: {follower.Rank} | Rank Points: {follower.RankingPoints}", SpriteGenerator(raftariob64)));
            }

            _customListTableView.ReloadData();
            _customListTableView.ScrollToCellWithIdx(0, TableView.ScrollPositionType.Beginning, false);
            _lastSelectedRow = -1;
        }

        public void TogglePageButtons(bool pageUpEnabled, bool pageDownEnabled)
        {
            _pageUpButton.interactable = pageUpEnabled;
            _pageDownButton.interactable = pageDownEnabled;
        }

        public void RemoveFollower(int selectedCell)
        {
            var toRemove = Followers[selectedCell];
            Utilities.Logger.Log.Info(toRemove.Country);
            Followers.Remove(toRemove);
            Refresh();
        }

        public Sprite SpriteGenerator(string spriteb64)
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(Convert.FromBase64String(spriteb64));
            tex.wrapMode = TextureWrapMode.Clamp;
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            
            return sprite;
        }

        public const string raftariob64 = "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";
    }
}

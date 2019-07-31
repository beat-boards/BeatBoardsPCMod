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
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;

namespace BeatBoards.UI
{
    public class BeatBoardsMenu : MonoBehaviour
    {
        public static void Load() { new GameObject("BeatBoards: Menu Manager").AddComponent<BeatBoardsMenu>(); }


        public TextMeshProUGUI loadingText;
        public CustomMenu beatBoardsMenu;
        public CustomMenu keyboardMenu;
        public CustomViewController mainViewController;
        public SearchKeyboardViewController keyboardViewController;

        private TextMeshProUGUI _identifierText;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _accountStatus;

        private TextMeshProUGUI _rankPointsText;
        private TextMeshProUGUI _rankText;
        private TextMeshProUGUI _roleText;

        private Button _editNameButton;
        private Button _editIconButton;


        private void Awake()
        {
            
        }

        private void Start()
        {


            beatBoardsMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Beat BoardS");
            keyboardMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Account Change");
            mainViewController = BeatSaberUI.CreateViewController<CustomViewController>();
            keyboardViewController = BeatSaberUI.CreateViewController<SearchKeyboardViewController>();
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

            _editNameButton = BeatSaberUI.CreateUIButton(mainViewController.rectTransform, "OkButton", new Vector2(60, 10));
            _editNameButton.ToggleWordWrapping(false);
            _editIconButton = BeatSaberUI.CreateUIButton(mainViewController.rectTransform, "OkButton", new Vector2(60, -10));
            _editIconButton.ToggleWordWrapping(false);

            mainViewController.backButtonPressed += MenuExited;

            Fetch();
        }

        private void MenuExited()
        {
            mainViewController.backButtonPressed -= MenuExited;
            Cleanup();
        }

        private void Fetch()
        {
            if (Core.Global.BeatBoardsID == "0" || Core.Global.BeatBoardsID.Length < 3)
            {
                loadingText.SetText("User Not Found. Play your first song or check your internet!");
                return;
            }

            loadingText.SetText("Fetching Data...");
            SharedCoroutineStarter.instance.StartCoroutine(GetData("http://beatboards.net/api/users?beatboardsID=" + Core.Global.BeatBoardsID));
        }

        IEnumerator GetData(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
            }
            else
            {
                loadingText.SetText("");

                APIModels.User user = JsonConvert.DeserializeObject<APIModels.User[]>(www.downloadHandler.text).First();

                string status = user.Banned.ToString();
                string rank = user.Rank.ToString();
                string role = user.Role;

                if (status == "False") status = "Active";
                else status = "<color=red>Banned</color>";

                if (rank == "69") rank = "<color=purple>6</color><color=#fa5cef>9</color> (Nice!)";

                if (role == "Owner") role = "<color=#00ffff>Owner</color>";
                else if (role == "Curator") role = "<color=#34ebb1>Curator</color>";

                PresentData(user.BeatBoardsID, user.UserData.Nickname, status, user.RankPoints.ToString(), rank, role, user.UserData.Image);
            }
        }

        private void PresentData(string id, string name, string status, string rankpoints, string globalrank, string role, string imageb64)
        {
            Logger.Log.Info("Test");
            beatBoardsMenu.SetMainViewController(mainViewController, true, (firstActivation, type) =>
            {
                _identifierText.SetText("<b>User ID:</b> " + id);
                _nameText.SetText("<b>Name:</b> " + name);
                _accountStatus.SetText("<b>Status:</b> " + status);
                _rankPointsText.SetText("<b>Rank Points:</b> " + rankpoints);
                _rankText.SetText("<b>Global Rank:</b> " + globalrank);
                _roleText.SetText("<b>Role:</b> " + role);
                var _userImage = new GameObject("BeatBoards: User Image").AddComponent<RawImage>();
                _userImage.material = CustomUI.Utilities.UIUtilities.NoGlowMaterial;
                _userImage.rectTransform.sizeDelta = new Vector2(28f, 28f);
                _userImage.rectTransform.SetParent(mainViewController.transform, false);
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(Convert.FromBase64String(imageb64));
                _userImage.texture = tex;
                _userImage.texture.mipMapBias = 0;
                _userImage.transform.localPosition = new Vector3(-51f, 1f);

                _editNameButton.onClick.AddListener(delegate { CreateUpdateNameKeyboard(); });
                _editIconButton.onClick.AddListener(delegate { CreateIconKeyboard(); });

                _editNameButton.SetButtonText("Edit Name");
                _editIconButton.SetButtonText("Edit Icon");

            });

            beatBoardsMenu.Present();
        }

        private void CreateUpdateNameKeyboard()
        {
            keyboardMenu.title = "Change Nickname";
            keyboardMenu.SetMainViewController(keyboardViewController, false, (firstActivation, type) =>
            {
                keyboardViewController.searchButtonPressed += UpdateNameKeyboardEnterPressed;
            });
            keyboardMenu.Present();
        }

        private void UpdateNameKeyboardEnterPressed(string obj)
        {
            keyboardMenu.Dismiss();
        }

        private void CreateIconKeyboard()
        {
            keyboardMenu.title = "Change Icon";
            keyboardMenu.SetMainViewController(keyboardViewController, false, (firstActivation, type) =>
            {
                keyboardViewController.searchButtonPressed += UpdateIconKeyboard;
            });
            keyboardMenu.Present();
        }

        private void UpdateIconKeyboard(string obj)
        {
            keyboardMenu.Dismiss();
        }

        public void Cleanup()
        {
            Destroy(this);
        }
    }
}

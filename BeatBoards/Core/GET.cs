using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BeatBoards.Utilities;
using Logger = BeatBoards.Utilities.Logger;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using BeatBoards.UI.ViewControllers;

namespace BeatBoards.Core
{
    public class GET : MonoBehaviour
    {
        private static GET _instance;
        public static GET Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Info("Initializing: BeatBoards GET Requests");
                    GameObject GETGameObject = new GameObject("BeatBoards: GET Requests Singleton");
                    _instance = GETGameObject.AddComponent<GET>();
                    DontDestroyOnLoad(GETGameObject);
                }
                return _instance;
            }
        }


        public void GetUserFromPlatformID(PlayerInfoViewController vc)
        {
            //StopAllCoroutines();
            StartCoroutine(GetUserEnumerator("cc0d001a-9441-4768-a5e8-56f0e2e612a4", vc));

            if (BS_Utils.Gameplay.GetUserInfo.GetUserID().ToString().StartsWith("765"))
            {
                //STEAM
            }
            else if (BS_Utils.Gameplay.GetUserInfo.GetUserID().ToString().Length == 16)
            {
                //OCULUS
            }
        }

        private IEnumerator GetUserEnumerator(string platformID, PlayerInfoViewController playerInfoViewController)
        {
            User currentPlayer = new User();
            UnityWebRequest www = UnityWebRequest.Get(Global.BeatBoardsAPIURL + "/users/" + platformID);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                yield return new WaitForSeconds(.05f);
            }
            else
            {
                currentPlayer = JsonConvert.DeserializeObject<User>(www.downloadHandler.text);
                playerInfoViewController.activeUser = currentPlayer;
                playerInfoViewController.LoadData();
            }
            yield break;
        }

        public void GetFriends(string UUID, FriendsListViewController vc) //I'm going to be using Enumerators so this is just a workaround since I can't return the value...
        {
            //StopAllCoroutines();
            StartCoroutine(FriendsEnumerator(UUID, vc));
        }

        private IEnumerator FriendsEnumerator(string UUID, FriendsListViewController friendsListViewController)
        {
            User currentPlayer = new User();
            friendsListViewController.Followers.Clear();

            UnityWebRequest www = UnityWebRequest.Get(Global.BeatBoardsAPIURL + "/users/" + UUID);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                yield return new WaitForSeconds(.05f);
            }
            else
            {
                currentPlayer = JsonConvert.DeserializeObject<User>(www.downloadHandler.text);
                foreach (var friendUUID in currentPlayer.Following)
                {
                    UnityWebRequest wwwFriend = UnityWebRequest.Get(Global.BeatBoardsAPIURL + "/users/" + friendUUID);
                    yield return wwwFriend.SendWebRequest();

                    if (wwwFriend.isNetworkError || wwwFriend.isHttpError)
                    {
                        Logger.Log.Error(wwwFriend.error);
                        yield return new WaitForSeconds(.05f);
                    }
                    else
                    {
                        var user = JsonConvert.DeserializeObject<User>(wwwFriend.downloadHandler.text);
                        friendsListViewController.Followers.Add(new Following()
                        {
                            Country = user.Country,
                            Fails = user.Fails,
                            Rank = 1,
                            RankingPoints = user.Rp,
                            Role = user.Role,
                            Username = user.Username,
                            Uuid = user.Id,
                            ImageB64 = user.Image
                        });

                        Logger.Log.Info(user.Country);
                    }
                }

                friendsListViewController.SetContent();
            }
        }
    }
}

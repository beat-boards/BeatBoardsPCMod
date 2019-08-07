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
using BS_Utils.Gameplay;

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


        private void GetLeaderboard()
        {

        }

        private IEnumerator GetLeaderboardEnumerator(string hash)
        {
            Map map = new Map();
            UnityWebRequest www = UnityWebRequest.Get(Global.BeatBoardsAPIURL + "/maps" + $"?hash=" + hash);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                yield return new WaitForSeconds(.05f);
            }
            else
            {
                map = JsonConvert.DeserializeObject<List<Map>>(www.downloadHandler.text).FirstOrDefault();
                


            }
            yield break;
        }


        public void GetUserFromPlatformID(PlayerInfoViewController vc)
        {
            //StopAllCoroutines();
            StopCoroutine(GetUserEnumerator(null, null, null));
            //StartCoroutine(GetUserEnumerator("cc0d001a-9441-4768-a5e8-56f0e2e612a4", vc));
            string id = GetUserInfo.GetUserID().ToString();

            if (id.StartsWith("7656"))
            {
                //STEAM
                StartCoroutine(GetUserEnumerator(id, vc, "steamId"));
            }
            else if (id.Length == 16)
            {
                //OCULUS
                StartCoroutine(GetUserEnumerator(id, vc, "oculusId"));
            }
        }

        private IEnumerator GetUserEnumerator(string platformID, PlayerInfoViewController playerInfoViewController, string type)
        {
            User currentPlayer = new User();
            UnityWebRequest www = UnityWebRequest.Get(Global.BeatBoardsAPIURL + "/users" + $"?{type}=" + platformID);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Logger.Log.Error(www.error);
                yield return new WaitForSeconds(.05f);
            }
            else
            {
                currentPlayer = JsonConvert.DeserializeObject<List<User>>(www.downloadHandler.text).FirstOrDefault();
                playerInfoViewController.activeUser = currentPlayer;
                playerInfoViewController.LoadData();
            }
            yield break;
        }

        public void GetFriends(string UUID, FriendsListViewController vc) //I'm going to be using Enumerators so this is just a workaround since I can't return the value...
        {
            StopCoroutine(FriendsEnumerator(null, null));
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

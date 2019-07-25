using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BeatBoards.Utilities
{
    public class UtilityManager : MonoBehaviour
    {
        private static UtilityManager _instance;
        public static UtilityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Logger.Log.Info("Initializing: BeatBoards Utility Manager");
                    GameObject eventsGameObject = new GameObject("BeatBoards: Utility Manager Singleton");
                    _instance = eventsGameObject.AddComponent<UtilityManager>();
                    DontDestroyOnLoad(eventsGameObject);
                }
                return _instance;
            }
        }

        public class LoadScripts
        {
            static public Dictionary<string, Sprite> _cachedSprites = new Dictionary<string, Sprite>();

            public static IEnumerator LoadSpriteCoroutine(string spritePath, Action<Sprite> done)
            {
                Texture2D tex;

                if (_cachedSprites.ContainsKey(spritePath))
                {
                    done?.Invoke(_cachedSprites[spritePath]);
                    yield break;
                }

                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(spritePath))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        
                    }
                    else
                    {
                        tex = DownloadHandlerTexture.GetContent(www);
                        yield return new WaitForSeconds(.05f);
                        var newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
                        _cachedSprites.Add(spritePath, newSprite);
                        done?.Invoke(newSprite);
                    }
                }
            }
        }
    }
}

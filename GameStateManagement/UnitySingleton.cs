/*
 * From http://redframe-game.com/blog/global-managers-with-generic-singletons/#comment-32424
 * 
 * */

using UnityEngine;
namespace GameStateManagement
{
    //[ExecuteInEditMode]
    public class UnitySingleton<T> : MonoBehaviour
    where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                        instance = CreateInstance();
                }

                return instance;
            }
        }
        
        public virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //make sure not to spam console 
        public void OnApplicationQuit()
        {
            Destroy(gameObject);
        }

        private static T CreateInstance()
        {
            GameObject obj = new GameObject();
            obj.hideFlags = HideFlags.HideAndDontSave;
            return obj.AddComponent<T>();
        }
    }

}

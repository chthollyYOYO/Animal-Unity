using UnityEngine;

namespace Framework
{
    /// <summary>
    /// µ¥Àý
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                if (instance == null)
                {
                    GameObject gameObject = new(typeof(T).Name);
                    gameObject.AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);

            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }
    }
}


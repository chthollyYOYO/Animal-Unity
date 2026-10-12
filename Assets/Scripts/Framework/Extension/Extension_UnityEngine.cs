namespace UnityEngine
{
    public static partial class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out var com))
                com = go.AddComponent<T>();
            return com;
        }

        public static void RemoveComponent<T>(this GameObject go) where T : Component
        {
            if (go.TryGetComponent<T>(out var com))
            {
                if (Application.isPlaying)
                    Object.Destroy(com);
                else
                    Object.DestroyImmediate(com);
            }
        }

        public static GameObject FindDeepChild(this GameObject go, string name)
        {
            Transform tf = go.transform.FindDeepChild(name);
            if (tf != null)
                return tf.gameObject;
            return null;
        }

        public static T GetOrAddComponent<T>(this Transform tf) where T : Component
        {
            return tf.gameObject.GetOrAddComponent<T>();
        }

        public static void RemoveComponent<T>(this Transform tf) where T : Component
        {
            tf.gameObject.RemoveComponent<T>();
        }

        public static Transform FindDeepChild(this Transform tf, string name, bool error = true)
        {
            Transform child = tf.Find(name);
            if (child != null)
            {
                return child;
            }

            foreach (Transform t in tf)
            {
                child = t.FindDeepChild(name);
                if (child != null)
                {
                    return child;
                }
            }

            Debug.LogError($"节点({tf.gameObject.name})下没有找到子节点({name})");
            return null;
        }

        public static Vector2Int ToVector2Int(this Vector2 v)
        {
            return new(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
        }

        public static Vector2 ToGridCenter(this Vector2 v)
        {
            return new(Mathf.FloorToInt(v.x) + 0.5f, Mathf.FloorToInt(v.y) + 0.5f);
        }

        public static Vector2 ToGridCenter(this Vector2Int v)
        {
            return new(v.x + 0.5f, v.y + 0.5f);
        }

        public static void SetLocalPositionX(this Transform tf, float x)
        {
            tf.localPosition = new Vector3(x, tf.localPosition.y, tf.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform tf, float y)
        {
            tf.localPosition = new Vector3(tf.localPosition.x, y, tf.localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform tf, float z)
        {
            tf.localPosition = new Vector3(tf.localPosition.x, tf.localPosition.y, z);
        }

        public static void SetLocalScaleX(this Transform tf, float x)
        {
            tf.localScale = new Vector3(x, tf.localScale.y, tf.localScale.z);
        }

        public static void SetLocalScaleY(this Transform tf, float y)
        {
            tf.localScale = new Vector3(tf.localScale.x, y, tf.localScale.z);
        }

        public static void SetLocalScaleZ(this Transform tf, float z)
        {
            tf.localScale = new Vector3(tf.localScale.x, tf.localScale.y, z);
        }

        public static void SetLocalEulerAnglesZ(this Transform tf, float z)
        {
            tf.localEulerAngles = new Vector3(tf.localEulerAngles.x, tf.localEulerAngles.y, z);
        }
    }
}

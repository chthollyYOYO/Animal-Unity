using Spine.Unity;
using UnityEngine;

namespace Framework
{
    public static partial class Helper
    {
        public static GameObject Instantiate(SkeletonDataAsset dataAsset)
        {
            GameObject go = new(dataAsset.name);
            var skeletonAnimation = go.AddComponent<SkeletonAnimation>();
            skeletonAnimation.skeletonDataAsset = dataAsset;
            skeletonAnimation.Initialize(true);
            return go;
        }

        public static bool RandomCheck(float probability)
        {
            return Random.value < probability;
        }

        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}

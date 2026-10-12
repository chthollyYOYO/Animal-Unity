using Framework;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BeatmapOnceCube : MonoBehaviour, IBeatmapChecker
{

    public GameObject cube;
    public static List<BeatmapOnceCube> pool = new();

    public static BeatmapOnceCube Create()
    {
        if (pool.Count > 0)
        {
            var result = pool[0];
            pool.RemoveAt(0);
            result.gameObject.SetActive(true);
            return result;
        } 

        GameObject prefab = null;
        if (Application.isPlaying)
        {
            prefab = ResourceManager.LoadAssetSync<GameObject>("Beatmap/Components/BeatmapOnceCube");
        }
        else
        {
#if UNITY_EDITOR
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Beatmap/Components/BeatmapOnceCube.prefab");
#endif
        }
        var gameObject = Instantiate(prefab);
        return gameObject.GetComponent<BeatmapOnceCube>();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        pool.Add(this);
    }

    public void SetStyle(BeatmapKeyCode keyCode)
    {

    }

    public void UpdateChecker(float timeOffset)
    {

    }

    public static bool IsVaild(float timeOffset)
    {
        return timeOffset < 0 && -timeOffset < IBeatmapChecker.DURATION;
    }

    public Vector3 position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public Quaternion rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }

    private void OnDestroy()
    {
        pool.Remove(this);
    }
}
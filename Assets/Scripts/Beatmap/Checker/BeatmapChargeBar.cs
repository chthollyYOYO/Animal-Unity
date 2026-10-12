using Framework;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;


[ExecuteAlways]

public class BeatmapChargeBar : MonoBehaviour, IBeatmapChecker
{
    public GameObject arrow;
    public GameObject bar;
    public float speed = 5f;

    private float barLength;
    
    public static List<BeatmapChargeBar> pool = new();
    public static BeatmapChargeBar Create()
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
            prefab = ResourceManager.LoadAssetSync<GameObject>("Beatmap/Components/BeatmapChargeBar");
        }
        else
        {
#if UNITY_EDITOR
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Beatmap/Components/BeatmapChargeBar.prefab");
#endif
        }
        var gameObject = Instantiate(prefab);
        return gameObject.GetComponent<BeatmapChargeBar>();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        pool.Add(this);
    }


    public static int IsVaild(float timeOffset, float duration)
    {
        if (timeOffset < 0 && -timeOffset < IBeatmapChecker.DURATION)
        {
            return 1;
        }
        else if (timeOffset >= 0 && timeOffset < duration)
        {
            return 2;
        }
        return 0;
    }
    public void UpdateChecker(float timeOffset, float Duration)
    {
        UpdateBar(Duration);
        UpdateArrow(timeOffset);
    }
    public void UpdateBar(float Duration)
    {
        barLength = Duration * speed;

        SpriteRenderer sr = bar.GetComponent<SpriteRenderer>();
        Vector2 size = sr.size;

        size.x = barLength;

        sr.size = size;
    }

    private void UpdateArrow(float timeOffset)
    {
        float distance = timeOffset * speed;

        distance = Mathf.Clamp(distance, 0, barLength);

        SetArrowPostion(distance);   
    }

    private void SetArrowPostion(float distance)
    {

        float left = -barLength * 0.5f;


        arrow.transform.SetLocalPositionX(left + distance);
       
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

    public void SetStyle(BeatmapKeyCode keyCode)
    {

    }
}
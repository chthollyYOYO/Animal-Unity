using Framework;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BeatmapOnce : MonoBehaviour, IBeatmapChecker
{
    private GameObject circle;
    public GameObject circleUp;
    public GameObject circleDown;
    public GameObject circleLeft;
    public GameObject circleRight;
    public GameObject circleSpace;
    public GameObject perfect;
    public GameObject miss;
    public GameObject clickCircle;

    public float clickCircleMaxScale = 2f;
    public float clickCircleMinScale = 0.5f;
    public float clickCircleDuration = 0.3f;

    public float maxScale;
    public float minScale;

    private float clickCircleTimer;
    private bool clickCirclePlaying;

    public GameObject arrowUp;
    public GameObject arrowDown;
    public GameObject arrowLeft;
    public GameObject arrowRight;
    public GameObject space;
    private BeatmapKeyCode tempSave;
    public static List<BeatmapOnce> pool = new();

    public static BeatmapOnce Create()
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
            prefab = ResourceManager.LoadAssetSync<GameObject>("Beatmap/Components/BeatmapOnce");
        }
        else
        {
#if UNITY_EDITOR
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Beatmap/Components/BeatmapOnce.prefab");
#endif
        }
        var gameObject = Instantiate(prefab);
        return gameObject.GetComponent<BeatmapOnce>();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        pool.Add(this);
    }

    public void SetStyle(BeatmapKeyCode keyCode)
    {
        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        space.SetActive(false);
        perfect.SetActive(false);
        miss.SetActive(false);
        clickCircle.SetActive(false);

        switch (keyCode)
        {
            case BeatmapKeyCode.W:
                arrowUp.SetActive(true);
                break;
            case BeatmapKeyCode.S:
                arrowDown.SetActive(true); 
                break;
            case BeatmapKeyCode.A:
                arrowLeft.SetActive(true);
                break;
            case BeatmapKeyCode.D:
                arrowRight.SetActive(true);
                break;
            case BeatmapKeyCode.Space:
                space.SetActive(true);
                break;
        }
        tempSave = keyCode;
    }

    public void UpdateChecker(float timeOffset)
    {
        float v = Mathf.Lerp(minScale, maxScale, -timeOffset / IBeatmapChecker.DURATION);
       

        circleUp.SetActive(false);
        circleDown.SetActive(false);
        circleLeft.SetActive(false);
        circleRight.SetActive(false);
        circleSpace.SetActive(false);

        switch (tempSave)
        {
            case BeatmapKeyCode.W:
                circle = circleUp;
                break;
            case BeatmapKeyCode.S:
                circle = circleDown;
                break;
            case BeatmapKeyCode.A:
                circle = circleLeft;
                break;
            case BeatmapKeyCode.D:
                circle = circleRight;
                break;
            case BeatmapKeyCode.Space:
                circle = circleSpace;
                break;
        }
        circle.SetActive(true);
        circle.transform.localScale = new(v, v, 1);
    }

    public void CloseCheckers()
    {
        circle.SetActive(false);

        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        space.SetActive(false);
    }

    public void PerfectClick()
    {
        CloseCheckers();
        StartClickCircle();
        perfect.SetActive(true);
    }

    public void MissClick()
    {
        CloseCheckers();
        MissPlay();
        miss.SetActive(true);
    }
    public void MissPlay()
    {
        clickCircleTimer = 0f;
        clickCirclePlaying = true;
    }

    public bool UpdateMiss(float deltaTime)
    {
        if (!clickCirclePlaying)
            return false;
        clickCircleTimer += deltaTime;
        float t = clickCircleTimer / clickCircleDuration;
        if (t >= 1f)
        {
            clickCirclePlaying = false;
            return true;
        }
        return false;
    }
    public void StartClickCircle()
    {
        clickCircle.SetActive(true);
        clickCircle.transform.localScale = Vector3.one * clickCircleMinScale;

        clickCircleTimer = 0f;
        clickCirclePlaying = true;
    }

    public bool UpdateClickCircle(float deltaTime)
    {
        if(!clickCirclePlaying)
            return false;

        clickCircleTimer += deltaTime;

        float t = clickCircleTimer / clickCircleDuration;
        float scale = Mathf.Lerp(clickCircleMinScale, clickCircleMaxScale, t);

        clickCircle.transform.localScale = new Vector3(scale, scale, 1);

        if (t >= 1f)
        {
            clickCircle.SetActive(false);
            clickCirclePlaying = false;
            return true;
        }

        return false;
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
using Framework;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BeatmapLongPress : MonoBehaviour, IBeatmapChecker
{
    private GameObject circle;
    public GameObject circleUp;
    public GameObject circleDown;
    public GameObject circleLeft;
    public GameObject circleRight;
    public GameObject circleSpace;
    public GameObject clickCircle;
    private BeatmapKeyCode tempSave;

    public float maxScale;
    public float minScale;

    public float clickCircleMaxScale = 2f;
    public float clickCircleMinScale = 0.5f;
    public float clickCircleDuration = 0.3f;

    private GameObject arrow;

    public GameObject arrowUp;
    public GameObject arrowDown;
    public GameObject arrowLeft;
    public GameObject arrowRight;
    public GameObject space;

    public GameObject targetArrowUp;
    public GameObject targetArrowDown;
    public GameObject targetArrowLeft;
    public GameObject targetArrowRight;
    public GameObject targetArrowSpace;

    public GameObject perfect;
    public GameObject miss;

    private float clickCircleTimer;
    private bool clickCirclePlaying;

    private GameObject targetArrow;
    public float speed;

    public static List<BeatmapLongPress> pool = new();

    public static BeatmapLongPress Create()
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
            prefab = ResourceManager.LoadAssetSync<GameObject>("Beatmap/Components/BeatmapLongPress");
        }
        else
        {
#if UNITY_EDITOR
            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Beatmap/Components/BeatmapLongPress.prefab");
#endif
        }
        var gameObject = Instantiate(prefab);
        return gameObject.GetComponent<BeatmapLongPress>();
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

        targetArrowUp.SetActive(false);
        targetArrowDown.SetActive(false);
        targetArrowLeft.SetActive(false);
        targetArrowRight.SetActive(false);
        targetArrowSpace.SetActive(false);
        clickCircle.SetActive(false);
        perfect.SetActive(false);
        miss.SetActive(false);
        switch (keyCode)
        {
            case BeatmapKeyCode.W:
                arrow = arrowUp;
                targetArrow = targetArrowUp;
                break;
            case BeatmapKeyCode.S:
                arrow = arrowDown;
                targetArrow = targetArrowDown;
                break;
            case BeatmapKeyCode.A:  
                arrow = arrowLeft;
                targetArrow = targetArrowLeft;
                break;
            case BeatmapKeyCode.D:
                arrow = arrowRight;
                targetArrow = targetArrowRight;
                break;
            case BeatmapKeyCode.Space:  
                arrow = space;
                targetArrow = targetArrowSpace;
                break;
        }
        arrow.SetActive(true);
        tempSave = keyCode;
        targetArrow.SetActive(true);
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

    public void Moving(float timeOffset, float duration)
    {
        circle.SetActive(false);
        float x = Mathf.Lerp(0, duration, timeOffset / duration);
        SetLocalPositionY(arrow.transform, x * speed);
        perfect.transform.localPosition = arrow.transform.localPosition;
        miss.transform.localPosition = arrow.transform.localPosition;
        clickCircle.transform.localPosition = arrow.transform.localPosition;
    }

    public void Checker(float timeOffset, float duration)
    {
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
        float scale = Mathf.Lerp(minScale, maxScale, -timeOffset / IBeatmapChecker.DURATION);
        circle.SetActive(true);
        circle.transform.localScale = new(scale, scale, 1);
        arrow.transform.localPosition = Vector3.zero;

        SetLocalPositionY(targetArrow.transform, duration * speed);
    }

    public void CloseCheckers()
    {
        circle.SetActive(false);
        arrow.SetActive(false);
        circle.SetActive(false);

        targetArrow.SetActive(false);
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
        if (!clickCirclePlaying)
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

    private void SetLocalPositionY(Transform tf, float y)
    {
        tf.localPosition = arrow.transform.rotation * (Vector2.up * y);
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
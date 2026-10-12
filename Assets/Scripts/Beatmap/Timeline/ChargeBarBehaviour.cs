using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChargeBarJudgePoint
{
    public float startTime;
    public float endTime;
}

[Serializable]
public class ChargeBarBehaviour : TimelinePlayableBehaviour, IBeatmapEmitter
{
    public BeatmapKeyCode keyCode;

    public float previewDuration = 4f;

    public List<ChargeBarJudgePoint> judgePoints = new();

    [HideInInspector]
    public Vector3 position;

    [HideInInspector]
    public Quaternion rotation;

    public BeatmapKeyCode KeyCode => keyCode;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public Quaternion Rotation
    {
        get => rotation;
        set => rotation = value;
    }
}
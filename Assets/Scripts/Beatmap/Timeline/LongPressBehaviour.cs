using System;
using UnityEngine;

[Serializable]
public class LongPressBehaviour : TimelinePlayableBehaviour, IBeatmapEmitter
{
    public BeatmapKeyCode keyCode;

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
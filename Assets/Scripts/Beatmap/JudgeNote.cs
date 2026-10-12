using UnityEngine;

public enum NoteType 
{
    Once,
    Hold,
    Pause,
}

public enum JudgeType
{
    Done,
    perfect,
    miss,
    none,
}

public class JudgeNote
{
    public NoteType Type;

    public float spawnTime;
    public float startTime;
    public float endTime;

    public BeatmapKeyCode keyCode;

    public Vector3 position;
    public Quaternion rotation;

    public JudgeType judged;
    public bool holding;

    public IBeatmapChecker checker = null;
    public float holdingStart;
}
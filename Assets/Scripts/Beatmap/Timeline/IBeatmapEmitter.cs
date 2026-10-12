using UnityEngine;

public interface IBeatmapEmitter
{
    BeatmapKeyCode KeyCode { get; }

    Vector3 Position { get; set; }

    Quaternion Rotation { get; set; }
}
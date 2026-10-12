using UnityEngine;

public interface IBeatmapPauseEmitter
{
    Vector3 Position { get; set; }

    Quaternion Rotation { get; set; }
}
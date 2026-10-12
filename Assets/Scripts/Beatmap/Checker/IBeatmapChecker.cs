using UnityEngine;

public interface IBeatmapChecker
{
    const float DURATION = 1f;

    void SetStyle(BeatmapKeyCode keyCode);

    void Destroy();

    Vector3 position { get; set; }

    Quaternion rotation { get; set; }
}
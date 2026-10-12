using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0f, 0f)]
[TrackClipType(typeof(LongPressClip))]
public class LongPressTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<LongPressBehaviour>.Create(graph, inputCount);
    }
}

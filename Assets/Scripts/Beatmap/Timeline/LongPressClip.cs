using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LongPressClip : PlayableAsset, ITimelineClipAsset
{
    public LongPressBehaviour template = new();

    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LongPressBehaviour>.Create(graph, template);
        return playable;
    }
}
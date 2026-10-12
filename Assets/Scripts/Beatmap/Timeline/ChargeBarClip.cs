using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ChargeBarClip : PlayableAsset, ITimelineClipAsset
{
    public ChargeBarBehaviour template = new();

    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ChargeBarBehaviour>.Create(graph, template);
        return playable;
    }
}
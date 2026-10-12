//using UnityEngine;
//using UnityEngine.Playables;
//using UnityEngine.Timeline;

//[TrackColor(0f, 0f, 0f)]
//[TrackClipType(typeof(ChargeBarClip))]

//public class ChargeBarTrack : TrackAsset
//{
//    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
//    {
//        return ScriptPlayable<ChargeBarBehaviour>.Create(graph, inputCount);
//    }
//}
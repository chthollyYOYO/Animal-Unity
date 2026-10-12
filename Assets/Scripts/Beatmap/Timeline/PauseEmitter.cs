using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PauseEmitter : Marker, INotification
{
    public PropertyName id => new(nameof(PauseEmitter));

}
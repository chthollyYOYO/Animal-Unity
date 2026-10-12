using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayableBehaviour : PlayableBehaviour
{
    public bool IsInBehaviour { get; private set; }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            var inputPlayable = (ScriptPlayable<TimelinePlayableBehaviour>)playable.GetInput(i);
            var behaviour = inputPlayable.GetBehaviour();
            var time = inputPlayable.GetTime();
            var speed = inputPlayable.GetSpeed();
            var weight = playable.GetInputWeight(i);
            var clipTime = (float)(time * speed * weight);
            if (Application.isPlaying)
            {
                if (clipTime > 0 && !behaviour.IsInBehaviour)
                {
                    behaviour.IsInBehaviour = true;
                    behaviour.OnEnter();
                }
                else if (clipTime == 0 && behaviour.IsInBehaviour)
                {
                    behaviour.IsInBehaviour = false;
                    behaviour.OnExit();
                }
            }
            behaviour.OnFrame(playable, info, playerData, clipTime, i);
        }
    }

    /// <summary>
    /// PlayMode Only
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// PlayMode Only
    /// </summary>
    public virtual void OnExit()
    {

    }

    public virtual void OnFrame(Playable playable, FrameData info, object playerData, float clipTime, int inputPort)
    {

    }
}
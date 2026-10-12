using cfg;

namespace GameLogic
{
    public class DialogueAction_Process_Cooking : DialogueAction
    {
        public DialogueAction_Process_Cooking(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            BeatmapSystem.Instance.StartBeat(1);
            BeatmapSystem.Instance.stopPlaying += OnStopPlaying;
        }

        public override void OnCompleted()
        {
            if (BeatmapSystem.Instance.isPlaying)
                BeatmapSystem.Instance.Stop();
            BeatmapSystem.Instance.stopPlaying -= OnStopPlaying;
        }

        private void OnStopPlaying()
        {
            isFinished = true;
        }
    }
}

using cfg;
using Framework;

namespace GameLogic
{
    public class DialogueAction_AmbienceStop : DialogueAction
    {
        public DialogueAction_AmbienceStop(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            AudioManager.Instance.StopAmbience(cfg.Param1);
        }
    }
}

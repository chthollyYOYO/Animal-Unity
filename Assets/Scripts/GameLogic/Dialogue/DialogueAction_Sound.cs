using cfg;
using Framework;

namespace GameLogic
{
    public class DialogueAction_Sound : DialogueAction
    {
        public DialogueAction_Sound(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            AudioManager.Instance.PlaySound(cfg.Param1);
        }
    }
}
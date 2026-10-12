using cfg;
using Framework;

namespace GameLogic
{
    public class DialogueAction_BgmStop : DialogueAction
    {
        public DialogueAction_BgmStop(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            AudioManager.Instance.StopBgm();
        }
    }
}

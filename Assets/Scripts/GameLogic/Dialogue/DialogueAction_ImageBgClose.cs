using cfg;
using Framework;
using UI;

namespace GameLogic
{
    public class DialogueAction_ImageBgClose : DialogueAction
    {
        public DialogueAction_ImageBgClose(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            UIManager.Instance.HidePanel<UI_Panel_DialogueImageBg>();
        }
    }
}
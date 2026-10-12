using cfg;
using Framework;
using UI;

namespace GameLogic
{
    public class DialogueAction_ExitBlackScreen : DialogueAction
    {
        public DialogueAction_ExitBlackScreen(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            UIManager.Instance.HidePanel<UI_Panel_BlackScreen>();
        }
    }
}

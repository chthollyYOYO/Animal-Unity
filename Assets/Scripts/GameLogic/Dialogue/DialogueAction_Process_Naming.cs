using cfg;
using Framework;
using System;
using UI;

namespace GameLogic
{
    public class DialogueAction_Process_Naming : DialogueAction
    {
        public DialogueAction_Process_Naming(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Naming, Action>(() => isFinished = true);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_Naming>();
        }
    }
}

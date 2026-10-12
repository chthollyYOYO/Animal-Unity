using cfg;
using Framework;
using System;
using UI;

namespace GameLogic
{
    public class DialogueAction_Process_Panel_Bookstore : DialogueAction
    {
        public DialogueAction_Process_Panel_Bookstore(DialogueCfg cfg) : base(cfg) { }

        public override bool isNeedStop => true;

        public override bool IsMeetStopCondition(DialogueCfg executedCfg) => false;

        public override void Execute()
        {
            isFinished = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Bookstore, Action>(() => isFinished = true);
        }

        public override void OnStop()
        {
            UIManager.Instance.HidePanel<UI_Panel_Bookstore>();
        }
    }
}

using cfg;
using Framework;
using System;
using UI;

namespace GameLogic
{
    public class DialogueAction_Process_Blinking : DialogueAction
    {
        public DialogueAction_Process_Blinking(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Anim_Blinking, Action>(() => isFinished = true);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_Anim_Blinking>();
        }
    }
}

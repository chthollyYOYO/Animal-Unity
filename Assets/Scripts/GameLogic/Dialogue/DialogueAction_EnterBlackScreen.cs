using cfg;
using Framework;
using UI;

namespace GameLogic
{
    public class DialogueAction_EnterBlackScreen : DialogueAction
    {
        public DialogueAction_EnterBlackScreen(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            UIManager.Instance.ShowPanelSync<UI_Panel_BlackScreen>();
        }


        public override bool isNeedStop => true;

        public override bool IsMeetStopCondition(DialogueCfg executedCfg)
        {
            return executedCfg.Action == DialogueActionType.exitBlackScreen;
        }

        public override void OnStop()
        {
            UIManager.Instance.HidePanel<UI_Panel_BlackScreen>();
        }
    }
}

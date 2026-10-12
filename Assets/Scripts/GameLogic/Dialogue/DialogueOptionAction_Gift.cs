using cfg;
using Framework;
using UI;

namespace GameLogic
{
    public class DialogueOptionAction_Gift : DialogueOptionAction
    {
        public DialogueOptionAction_Gift(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            GiftBagParam param = new()
            {
                OnConfirm = itemId => isFinished = true
            };
            UIManager.Instance.ShowPanelSync<UI_Panel_GiftBag, GiftBagParam>(param);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_GiftBag>();
        }

        public override void Stop()
        {
            UIManager.Instance.HidePanel<UI_Panel_GiftBag>();
        }
    }
}

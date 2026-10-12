using cfg;
using FairyGUI;
using Framework;
using UI;

namespace GameLogic
{
    public class DialogueOptionAction_BuyRecipe : DialogueOptionAction
    {
        public DialogueOptionAction_BuyRecipe(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            UIManager.Instance.ShowPanelSync<UI_Panel_Recipe>();
            UIManager.Instance.OnPanelHide += OnPanelHide;
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_Recipe>();
            UIManager.Instance.OnPanelHide -= OnPanelHide;
        }

        public override void Stop()
        {
            UIManager.Instance.HidePanel<UI_Panel_Recipe>();
            UIManager.Instance.OnPanelHide -= OnPanelHide;
        }

        private void OnPanelHide(GComponent ui)
        {
            if (ui is UI_Panel_Recipe)
            {
                isFinished = true;
            }
        }
    }
}

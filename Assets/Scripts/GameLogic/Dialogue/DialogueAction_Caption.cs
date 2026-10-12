using cfg;
using Framework;
using System;
using UI;

namespace GameLogic
{
    public struct DialogueCaptionContext
    {
        public string content;
        public Action onFinished;
    }

    public class DialogueAction_Caption : DialogueAction
    {
        public DialogueAction_Caption(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            DialogueCaptionContext context = new()
            {
                content = DialogueTextReplace.Replace(cfg.Content),
                onFinished = () => isFinished = true,
            };
            UIManager.Instance.ShowPanelSync<UI_Panel_Caption, DialogueCaptionContext>(context);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_Caption>();
        }
    }
}
using cfg;
using Framework;
using System;
using UI;
using UnityEngine;

namespace GameLogic
{
    public struct DialogueContext
    {
        public string name;
        public string content;
        public Texture2D roleImage;
        public Action onTextPlayEnd;
        public Action onNext;
    }

    public class DialogueAction_Chat : DialogueAction
    {
        public DialogueAction_Chat(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            //isShowOption = false;
            isFinished = false;

            Texture2D roleImage = null;
            if (!string.IsNullOrEmpty(cfg.Param1))
            {
                var sprite = ResourceManager.LoadAssetSync<Sprite>(cfg.Param1);
                if (sprite != null)
                    roleImage = sprite.texture;
            }

            DialogueContext context = new()
            {
                name = DialogueTextReplace.Replace(cfg.SpeakerName),
                content = DialogueTextReplace.Replace(cfg.Content),
                roleImage = roleImage,
                //onTextPlayEnd = () => isShowOption = true,
                onNext = () => isFinished = true,
            };
            UIManager.Instance.ShowPanelSync<UI_Panel_Dialogue, DialogueContext>(context);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_Dialogue>();
        }
    }
}

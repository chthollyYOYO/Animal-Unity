using cfg;
using Framework;
using System;
using UI;
using UnityEngine;

namespace GameLogic
{
    public struct DialogueImageContext
    {
        public Texture2D texture;
        public Action onNext;
    }

    public class DialogueAction_Image : DialogueAction
    {
        public DialogueAction_Image(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            isFinished = false;
            var sprite = ResourceManager.LoadAssetSync<Sprite>(cfg.Param1);
            DialogueImageContext context = new()
            {
                texture = sprite == null ? null : sprite.texture,
                onNext = () => isFinished = true,
            };
            UIManager.Instance.ShowPanelSync<UI_Panel_DialogueImage, DialogueImageContext>(context);
        }

        public override void OnCompleted()
        {
            UIManager.Instance.HidePanel<UI_Panel_DialogueImage>();
        }
    }
}
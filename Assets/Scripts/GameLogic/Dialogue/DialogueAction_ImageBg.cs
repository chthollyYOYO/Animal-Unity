using cfg;
using Framework;
using UI;
using UnityEngine;

namespace GameLogic
{
    public class DialogueAction_ImageBg : DialogueAction
    {
        public DialogueAction_ImageBg(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            var sprite = ResourceManager.LoadAssetSync<Sprite>(cfg.Param1);
            var texture = sprite == null ? null : sprite.texture;
            UIManager.Instance.ShowPanelSync<UI_Panel_DialogueImageBg, Texture2D>(texture);
        }


        public override bool isNeedStop => true;

        public override bool IsMeetStopCondition(DialogueCfg executedCfg)
        {
            return executedCfg.Action == DialogueActionType.image_bgClose && executedCfg.Param1 == cfg.Param1;
        }

        public override void OnStop()
        {
            UIManager.Instance.HidePanel<UI_Panel_DialogueImageBg>();
        }
    }
}
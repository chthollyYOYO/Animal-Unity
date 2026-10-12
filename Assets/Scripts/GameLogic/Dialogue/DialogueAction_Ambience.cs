using cfg;
using Framework;

namespace GameLogic
{
    public class DialogueAction_Ambience : DialogueAction
    {
        public DialogueAction_Ambience(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            AudioManager.Instance.PlayAmbience(cfg.Param1);
        }


        public override bool isNeedStop => true;

        public override bool IsMeetStopCondition(DialogueCfg executedCfg)
        {
            return executedCfg.Action == DialogueActionType.ambienceStop && executedCfg.Param1 == cfg.Param1;
        }

        public override void OnStop()
        {
            AudioManager.Instance.StopAmbience(cfg.Param1);
        }
    }
}

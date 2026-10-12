using cfg;
using Framework;

namespace GameLogic
{
    public class DialogueAction_Bgm : DialogueAction
    {
        public DialogueAction_Bgm(DialogueCfg cfg) : base(cfg) { }

        public override void Execute()
        {
            AudioManager.Instance.PlayBgm(cfg.Param1);
        }


        public override bool isNeedStop => true;

        public override bool IsMeetStopCondition(DialogueCfg executedCfg)
        {
            return executedCfg.Action == DialogueActionType.bgmStop;
        }

        public override void OnStop()
        {
            AudioManager.Instance.StopBgm();
        }
    }
}

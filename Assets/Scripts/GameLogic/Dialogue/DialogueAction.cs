using cfg;

namespace GameLogic
{
    public abstract class DialogueAction
    {
        public DialogueCfg cfg;

        public DialogueAction(DialogueCfg cfg)
        {
            this.cfg = cfg;
        }
        
        /// <summary>
        /// 当动作完成时，将此值置为true
        /// </summary>
        public bool isFinished = true;

        ///// <summary>
        ///// 如需在动作进行时显示选项，将此值置为false
        ///// 当可以显示选项时，将此值置为true
        ///// </summary>
        //public bool? isShowOption;

        public abstract void Execute();

        public virtual void OnCompleted()
        {

        }

        //---中断处理---
        // 成对的功能如黑屏进出、环境音播放等，当执行了开始未执行结束时，对话被中断。
        /// <summary>
        /// 是否需要中断处理
        /// </summary>
        public virtual bool isNeedStop => false;

        /// <summary>
        /// 执行过何种动作后，不需要中断处理
        /// </summary>
        public virtual bool IsMeetStopCondition(DialogueCfg executedCfg) => true;

        /// <summary>
        /// 中断处理
        /// </summary>
        public virtual void OnStop() { }
        //---End---
    }
}

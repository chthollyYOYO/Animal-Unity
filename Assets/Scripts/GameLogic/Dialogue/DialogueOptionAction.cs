using cfg;

namespace GameLogic
{
    public abstract class DialogueOptionAction
    {
        public DialogueCfg cfg;

        public DialogueOptionAction(DialogueCfg cfg)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// 当动作完成时，将此值置为true
        /// </summary>
        public bool isFinished = true;

        public abstract void Execute();

        public virtual void OnCompleted() {  }

        /// <summary>
        /// 中断处理
        /// </summary>
        public virtual void Stop() { }
    }
}

using System.Threading;

namespace Framework
{
    public class NodeCfg_Action : NodeCfg
    {
        public int[] lastNodeIds { get; set; }
        public int[] nextNodeIds { get; set; }
        public TriggerActionCfg[] actions { get; set; }
    }

    public class Node_Action : Node
    {
        private NodeCfg_Action cfg;
        private TriggerAction[] actions;
        private CancellationTokenSource cts = new();

        protected override void Init(NodeCfg cfg)
        {
            base.Init(cfg);
            this.cfg = cfg as NodeCfg_Action;
            actions = TriggerAction.CreateAll(this.cfg.actions);
        }

        public override bool IsStartNode()
        {
            return cfg.lastNodeIds == null || cfg.lastNodeIds.Length == 0;
        }

        public override async void Execute()
        {
            await TriggerAction.ExecuteAll(actions, cts.Token);
            mgr.OnNodeExecuteCompleted(cfg.nodeId, cfg.nextNodeIds);
        }

        public override void Stop()
        {
            cts.Cancel();
        }
    }
}

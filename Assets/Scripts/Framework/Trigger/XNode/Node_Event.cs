using Cysharp.Threading.Tasks;
using System.Threading;

namespace Framework
{
    public class NodeCfg_Event : NodeCfg
    {
        public int[] lastNodeIds { get; set; }
        public int[] nextNodeIds { get; set; }
        public TriggerEventCfg[] events { get; set; }
    }

    public class Node_Event : Node
    {
        private NodeCfg_Event cfg;
        private TriggerEvent[] evts;
        private bool isReceivedEvent;
        private CancellationTokenSource cts = new();

        protected override void Init(NodeCfg cfg)
        {
            base.Init(cfg);
            this.cfg = cfg as NodeCfg_Event;
            evts = TriggerEvent.CreateAll(this.cfg.events);
        }

        public override bool IsStartNode()
        {
            return cfg.lastNodeIds == null || cfg.lastNodeIds.Length == 0;
        }

        public override async void Execute()
        {
            isReceivedEvent = false;
            TriggerEvent.AddEventListeners(evts, OnReceived);
            await UniTask.WaitUntil(() => isReceivedEvent, cancellationToken: cts.Token);
            mgr.OnNodeExecuteCompleted(cfg.nodeId, cfg.nextNodeIds);
        }

        public override void Stop()
        {
            cts.Cancel();
            TriggerEvent.RemoveEventListeners(evts, OnReceived);
        }

        private void OnReceived()
        {
            isReceivedEvent = true;
            TriggerEvent.RemoveEventListeners(evts, OnReceived);
        }
    }
}

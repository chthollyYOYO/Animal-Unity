namespace Framework
{
    public class NodeCfg_Condition : NodeCfg
    {
        public int[] lastNodeIds { get; set; }
        public int[] nextNodeIdsIfTrue { get; set; }
        public int[] nextNodeIdsIfFalse { get; set; }
        public TriggerConditionCfg[] conditions { get; set; }
    }

    public class Node_Condition : Node
    {
        private NodeCfg_Condition cfg;
        private TriggerCondition[] conditions;

        protected override void Init(NodeCfg cfg)
        {
            base.Init(cfg);
            this.cfg = cfg as NodeCfg_Condition;
            conditions = TriggerCondition.CreateAll(this.cfg.conditions);
        }

        public override bool IsStartNode()
        {
            return cfg.lastNodeIds == null || cfg.lastNodeIds.Length == 0;
        }

        public override void Execute()
        {
            bool isMeet = TriggerCondition.IsMeetConditions(conditions);
            if (isMeet)
            {
                mgr.OnNodeExecuteCompleted(cfg.nodeId, cfg.nextNodeIdsIfTrue);
            }
            else
            {
                mgr.OnNodeExecuteCompleted(cfg.nodeId, cfg.nextNodeIdsIfFalse);
            }
        }
    }
}

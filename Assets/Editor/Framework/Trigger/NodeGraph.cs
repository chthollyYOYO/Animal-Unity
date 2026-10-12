using Framework;
using System;

namespace GameEditor
{
    [Serializable]
    public abstract class NodeGraph : XNode.NodeGraph
    {
        public abstract Type actionCfgType { get; }
        public abstract Type conditionCfgType { get; }
        public abstract Type eventCfgType { get; }

        public NodeCfg[] ToRuntimeCfg()
        {
            NodeCfg[] result = new NodeCfg[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is Node_Event eventNode)
                {
                    result[i] = eventNode.ToRuntimeCfg();
                }
                if (nodes[i] is Node_Condition conditionNode)
                {
                    result[i] = conditionNode.ToRuntimeCfg();
                }
                else if (nodes[i] is Node_Action actionNode)
                {
                    result[i] = actionNode.ToRuntimeCfg();
                }
            }
            return result;
        }
    }

    [Serializable]
    public class NodeGraph<TEventCfg, TConditionCfg, TActionCfg> : NodeGraph
        where TEventCfg : TriggerEventCfg
        where TConditionCfg : TriggerConditionCfg
        where TActionCfg : TriggerActionCfg
    {
        public override Type actionCfgType => typeof(TActionCfg);
        public override Type conditionCfgType => typeof(TConditionCfg);
        public override Type eventCfgType => typeof(TEventCfg);
    }
}

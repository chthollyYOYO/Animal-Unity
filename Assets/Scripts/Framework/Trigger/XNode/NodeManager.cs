using System.Collections.Generic;

namespace Framework
{
    public class NodeManager
    {
        private Dictionary<int, Node> nodeDict = new();
        private List<int> executingNodeIds = new();

        public void Init(NodeCfg[] cfgs)
        {
            for (int i = 0; i < cfgs.Length; i++)
            {
                var cfg = cfgs[i];
                var id = cfg.nodeId;
                if (nodeDict.ContainsKey(id))
                {
                    LogManager.Error($"节点ID重复: {id}");
                    continue;
                }
                else
                {
                    var node = Helper.CreateSubclass<Node>(cfg);
                    node.Init(cfg, this);
                    nodeDict.Add(id, node);
                }
            }
        }

        public void Run()
        {
            foreach (var kv in nodeDict)
            {
                var node = kv.Value;
                if (node.IsStartNode())
                {
                    executingNodeIds.Add(kv.Key);
                    node.Execute();
                }
            }
        }

        public void Stop()
        {
            foreach (var key in executingNodeIds)
                nodeDict[key].Stop();
            executingNodeIds.Clear();
        }

        public void OnNodeExecuteCompleted(int nodeId, int[] nextIds)
        {
            executingNodeIds.Remove(nodeId);
            if (nextIds != null && nextIds.Length > 0)
            {
                foreach (var nextId in nextIds)
                {
                    executingNodeIds.Add(nextId);
                    nodeDict[nextId].Execute();
                }
            }
        }
    }
}

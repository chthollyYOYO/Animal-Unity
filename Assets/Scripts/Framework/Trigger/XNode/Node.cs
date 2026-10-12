using System.Text.Json.Serialization;

namespace Framework
{
    [JsonConverter(typeof(SubclassJsonConverter<NodeCfg>))]
    public class NodeCfg
    {
        public int nodeId { get; set; }
    }

    public abstract class Node
    {
        protected NodeManager mgr;

        public void Init(NodeCfg cfg, NodeManager mgr)
        {
            this.mgr = mgr;
            Init(cfg);
        }

        protected virtual void Init(NodeCfg cfg)
        {

        }

        public abstract bool IsStartNode();

        public abstract void Execute();

        public virtual void Stop()
        {

        }
    }
}

using Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor
{
    [Serializable]
    [NodeWidth(300)]
    public class Node_Event : XNode.Node, ISerializationCallbackReceiver
    {
        [LabelText("描述")]
        [LabelWidth(30)]
        public string desc;

        [Input(ShowBackingValue.Never)]
        [HideLabel]
        public string last;

        [ValueDropdown("GetValueDropdownList")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("事件")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerEventCfg> events = new();
        [HideInInspector]
        [SerializeField]
        private string eventsJson;

        [Output]
        [HideLabel]
        public string next;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            eventsJson = Helper.JsonSerialize(events);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            events = Helper.JsonDeserialize<List<TriggerEventCfg>>(eventsJson);
        }

        public NodeCfg_Event ToRuntimeCfg()
        {
            NodeCfg_Event cfg = new();
            cfg.nodeId = graph.nodes.IndexOf(this);
            cfg.lastNodeIds = EditorHelper.GetConnectionNodeIds(GetPort("last"));
            cfg.nextNodeIds = EditorHelper.GetConnectionNodeIds(GetPort("next"));
            cfg.events = events.ToArray();
            return cfg;
        }

        private ValueDropdownList<TriggerEventCfg> GetValueDropdownList()
        {
            var graph = this.graph as NodeGraph;
            var result = Helper.GetSubclassesDropdownList<TriggerEventCfg>(graph.eventCfgType);
            return result;
        }
    }
}

using Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor
{
    [Serializable]
    [NodeWidth(300)]
    public class Node_Action : XNode.Node, ISerializationCallbackReceiver
    {
        [LabelText("描述")]
        [LabelWidth(30)]
        public string desc;

        [Input(ShowBackingValue.Never)]
        [HideLabel]
        public string last;

        [ValueDropdown("GetValueDropdownList")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("动作")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerActionCfg> actions = new();
        [HideInInspector]
        [SerializeField]
        private string actionsJson;

        [Output]
        [HideLabel]
        public string next;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            actionsJson = Helper.JsonSerialize(actions);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            actions = Helper.JsonDeserialize<List<TriggerActionCfg>>(actionsJson);
        }

        public NodeCfg_Action ToRuntimeCfg()
        {
            NodeCfg_Action cfg = new();
            cfg.nodeId = graph.nodes.IndexOf(this);
            cfg.lastNodeIds = EditorHelper.GetConnectionNodeIds(GetPort("last"));
            cfg.nextNodeIds = EditorHelper.GetConnectionNodeIds(GetPort("next"));
            cfg.actions = actions.ToArray();
            return cfg;
        }

        private ValueDropdownList<TriggerActionCfg> GetValueDropdownList()
        {
            var graph = this.graph as NodeGraph;
            var result = Helper.GetSubclassesDropdownList<TriggerActionCfg>(graph.actionCfgType);
            return result;
        }
    }
}

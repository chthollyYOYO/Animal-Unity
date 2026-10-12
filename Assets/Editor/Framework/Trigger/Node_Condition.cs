using Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor
{
    [Serializable]
    [NodeWidth(300)]
    public class Node_Condition : XNode.Node, ISerializationCallbackReceiver
    {
        [LabelText("描述")]
        [LabelWidth(30)]
        public string desc;

        [Input(ShowBackingValue.Never)]
        [HideLabel]
        public string last;

        [ValueDropdown("GetValueDropdownList")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("条件")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerConditionCfg> conditions = new();
        [HideInInspector]
        [SerializeField]
        private string conditionsJson;

        [Output]
        [LabelText("True")]
        public string nextIfTrue;

        [Output]
        [LabelText("False")]
        public string nextIfFalse;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            conditionsJson = Helper.JsonSerialize(conditions);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            conditions = Helper.JsonDeserialize<List<TriggerConditionCfg>>(conditionsJson);
        }

        public NodeCfg_Condition ToRuntimeCfg()
        {
            NodeCfg_Condition cfg = new();
            cfg.nodeId = graph.nodes.IndexOf(this);
            cfg.lastNodeIds = EditorHelper.GetConnectionNodeIds(GetPort("last"));
            cfg.nextNodeIdsIfTrue = EditorHelper.GetConnectionNodeIds(GetPort("nextIfTrue"));
            cfg.nextNodeIdsIfFalse = EditorHelper.GetConnectionNodeIds(GetPort("nextIfFalse"));
            cfg.conditions = conditions.ToArray();
            return cfg;
        }

        private ValueDropdownList<TriggerConditionCfg> GetValueDropdownList()
        {
            var graph = this.graph as NodeGraph;
            var result = Helper.GetSubclassesDropdownList<TriggerConditionCfg>(graph.conditionCfgType);
            return result;
        }
    }
}

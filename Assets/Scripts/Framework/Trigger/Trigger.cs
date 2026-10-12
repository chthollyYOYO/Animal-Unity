using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public abstract class TriggerCfg : ISerializationCallbackReceiver
    {
        [ValueDropdown("GetEventCfgs")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("事件")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerEventCfg> events = new();
        [HideInInspector]
        [SerializeField]
        private string eventsJson;

        [ValueDropdown("GetConditionCfgs")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("条件")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerConditionCfg> conditions = new();
        [HideInInspector]
        [SerializeField]
        private string conditionsJson;

        [ValueDropdown("GetActionCfgs")]
        [ListDrawerSettings(Expanded = true)]
        [LabelText("动作")]
        [ShowInInspector]
        [NonSerialized]
        public List<TriggerActionCfg> actions = new();
        [HideInInspector]
        [SerializeField]
        private string actionsJson;

        protected abstract ValueDropdownList<TriggerEventCfg> GetEventCfgs();
        protected abstract ValueDropdownList<TriggerConditionCfg> GetConditionCfgs();
        protected abstract ValueDropdownList<TriggerActionCfg> GetActionCfgs();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            eventsJson = Helper.JsonSerialize(events);
            conditionsJson = Helper.JsonSerialize(conditions);
            actionsJson = Helper.JsonSerialize(actions);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            events = Helper.JsonDeserialize<List<TriggerEventCfg>>(eventsJson);
            conditions = Helper.JsonDeserialize<List<TriggerConditionCfg>>(conditionsJson);
            actions = Helper.JsonDeserialize<List<TriggerActionCfg>>(actionsJson);
        }
    }

    [Serializable]
    public class TriggerCfg<TEventCfg, TConditionCfg, TActionCfg> : TriggerCfg
    {
        protected override ValueDropdownList<TriggerEventCfg> GetEventCfgs()
        {
            var result = Helper.GetSubclassesDropdownList<TriggerEventCfg>(typeof(TEventCfg));
            return result;
        }

        protected override ValueDropdownList<TriggerConditionCfg> GetConditionCfgs()
        {
            var result = Helper.GetSubclassesDropdownList<TriggerConditionCfg>(typeof(TConditionCfg));
            return result;
        }

        protected override ValueDropdownList<TriggerActionCfg> GetActionCfgs()
        {
            var result = Helper.GetSubclassesDropdownList<TriggerActionCfg>(typeof(TActionCfg));
            return result;
        }
    }

    public class Trigger
    {
        private TriggerEvent[] evts;
        private TriggerCondition[] conditions;
        private TriggerAction[] actions;
        private CancellationTokenSource cts = new();

        public void Init(TriggerCfg cfg)
        {
            evts = TriggerEvent.CreateAll(cfg.events.ToArray());
            conditions = TriggerCondition.CreateAll(cfg.conditions.ToArray());
            actions = TriggerAction.CreateAll(cfg.actions.ToArray());
        }

        public void Run()
        {
            if (evts == null || evts.Length == 0)
            {
                Execute();
            }
            else
            {
                TriggerEvent.AddEventListeners(evts, OnReceived);
            }
        }

        public void Stop()
        {
            TriggerEvent.RemoveEventListeners(evts, OnReceived);
            cts.Cancel();
        }

        private void Execute()
        {
            if (TriggerCondition.IsMeetConditions(conditions))
                TriggerAction.ExecuteAll(actions, cts.Token).Forget();
        }

        private void OnReceived()
        {
            TriggerEvent.RemoveEventListeners(evts, OnReceived);
            Execute();
        }
    }
}
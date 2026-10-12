using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Text.Json.Serialization;
using System.Threading;

namespace Framework.Example
{
    //事件基类
    [JsonConverter(typeof(SubclassJsonConverter<ExampleEventCfg>))]
    public abstract class ExampleEventCfg : TriggerEventCfg
    {
        public int evt_base;
    }

    public abstract class ExampleEvent : TriggerEvent
    {

    }

    [LabelText("事件A")]
    public class ExampleEventCfg_A : ExampleEventCfg
    {
        public int evt_a;
    }

    public class ExampleEvent_A : ExampleEvent
    {
        public override void AddListener(Action onReceived)
        {
            throw new NotImplementedException();
        }

        public override void RemoveListener(Action onReceived)
        {
            throw new NotImplementedException();
        }
    }

    //条件基类
    [JsonConverter(typeof(SubclassJsonConverter<ExampleConditionCfg>))]
    public abstract class ExampleConditionCfg : TriggerConditionCfg
    {
        public int cdt_base;
    }

    public abstract class ExampleCondition : TriggerCondition
    {

    }

    [LabelText("条件A")]
    public class ExampleConditionCfg_A : ExampleConditionCfg
    {
        public int cdt_a;
    }

    public class ExampleCondition_A : ExampleCondition
    {
        public override bool IsMeetCondition()
        {
            throw new NotImplementedException();
        }
    }

    //动作基类
    [JsonConverter(typeof(SubclassJsonConverter<ExampleActionCfg>))]
    public abstract class ExampleActionCfg : TriggerActionCfg
    {
        public int act_base;
    }

    public abstract class ExampleAction : TriggerAction
    {

    }

    [LabelText("动作A")]
    public class ExampleActionCfg_A : ExampleActionCfg
    {
        public int act_a;
    }

    public class ExampleAction_A : ExampleAction
    {
        public override async UniTask Action(CancellationToken ct)
        {
            await UniTask.Delay(1, cancellationToken: ct);
        }
    }
}
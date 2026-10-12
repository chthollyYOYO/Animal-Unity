using System;
using System.Text.Json.Serialization;

namespace Framework
{
    [JsonConverter(typeof(SubclassJsonConverter<TriggerEventCfg>))]
    public abstract class TriggerEventCfg
    {

    }

    public abstract class TriggerEvent
    {
        public virtual void Init(TriggerEventCfg cfg)
        {
            
        }

        public abstract void AddListener(Action onReceived);

        public abstract void RemoveListener(Action onReceived);

        public static TriggerEvent[] CreateAll(TriggerEventCfg[] cfgs)
        {
            if (cfgs == null)
                return null;

            var result = new TriggerEvent[cfgs.Length];
            for (int i = 0; i < cfgs.Length; i++)
            {
                var cfg = cfgs[i];
                var evt = Helper.CreateSubclass<TriggerEvent>(cfg);
                evt.Init(cfg);
                result[i] = evt;
            }
            return result;
        }

        public static void AddEventListeners(TriggerEvent[] evts, Action onReceived)
        {
            if (evts == null) 
                return;

            for (int i = 0; i < evts.Length; i++)
                evts[i].AddListener(onReceived);
        }

        public static void RemoveEventListeners(TriggerEvent[] evts, Action onReceived)
        {
            if (evts == null)
                return;

            for (int i = 0; i < evts.Length; i++)
                evts[i].RemoveListener(onReceived);
        }
    }
}

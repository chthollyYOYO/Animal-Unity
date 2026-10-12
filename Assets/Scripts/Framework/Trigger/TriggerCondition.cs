using System.Text.Json.Serialization;

namespace Framework
{
    [JsonConverter(typeof(SubclassJsonConverter<TriggerConditionCfg>))]
    public abstract class TriggerConditionCfg
    {
        
    }

    public abstract class TriggerCondition
    {
        public virtual void Init(TriggerConditionCfg cfg)
        {
            
        }

        public abstract bool IsMeetCondition();

        public static TriggerCondition[] CreateAll(TriggerConditionCfg[] cfgs)
        {
            if (cfgs == null)
                return null;

            var result = new TriggerCondition[cfgs.Length];
            for (int i = 0; i < cfgs.Length; i++)
            {
                var cfg = cfgs[i];
                var cdt = Helper.CreateSubclass<TriggerCondition>(cfg);
                cdt.Init(cfg);
                result[i] = cdt;
            }
            return result;
        }

        public static bool IsMeetConditions(TriggerCondition[] conditions)
        {
            if (conditions == null)
                return true;

            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].IsMeetCondition())
                {
                    return false;
                }
            }
            return true;
        }
    }
}

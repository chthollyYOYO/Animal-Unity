using Cysharp.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading;

namespace Framework
{
    [JsonConverter(typeof(SubclassJsonConverter<TriggerActionCfg>))]
    public abstract class TriggerActionCfg
    {

    }

    public abstract class TriggerAction
    {
        public virtual void Init(TriggerActionCfg cfg)
        {
            
        }

        public abstract UniTask Action(CancellationToken ct);

        public static TriggerAction[] CreateAll(TriggerActionCfg[] cfgs)
        {
            if (cfgs == null)
                return null;

            var result = new TriggerAction[cfgs.Length];
            for (int i = 0; i < cfgs.Length; i++)
            {
                var cfg = cfgs[i];
                var act = Helper.CreateSubclass<TriggerAction>(cfg);
                act.Init(cfg);
                result[i] = act;
            }
            return result;
        }

        public static async UniTask ExecuteAll(TriggerAction[] actions, CancellationToken ct)
        {
            if (actions != null && actions.Length > 0)
            {
                var tasks = new UniTask[actions.Length];
                for (int i = 0; i < actions.Length; i++)
                    tasks[i] = actions[i].Action(ct);
                await UniTask.WhenAll(tasks);
            }
        }
    }
}

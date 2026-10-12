using Framework;
using System.Collections.Generic;

namespace GameLogic
{
    public class SceneFlagManager : Singleton<SceneFlagManager>
    {
        public Dictionary<SceneFlagType, Dictionary<int, SceneFlag>> flags = new();

        public void RegisterFlag(SceneFlag flag)
        {
            if (!flags.TryGetValue(flag.flagType, out var dict))
                dict = flags[flag.flagType] = new();

            if (dict.ContainsKey(flag.id))
                LogManager.Error($"场景标记ID重复：{flag.flagType} {flag.id}");
            else
                dict[flag.id] = flag;
        }

        public void UnregisterFlag(SceneFlag flag)
        {
            if (flags.TryGetValue(flag.flagType, out var dict) && dict.ContainsKey(flag.id))
                dict.Remove(flag.id);
        }

        public SceneFlag GetFlag(SceneFlagType flagType, int id = 0)
        {
            if (flags.TryGetValue(flagType, out var dict) && dict.TryGetValue(id, out var flag))
                return flag;
            return null;
        }
    }
}

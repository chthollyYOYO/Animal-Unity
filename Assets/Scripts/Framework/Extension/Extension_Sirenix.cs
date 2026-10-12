using Sirenix.OdinInspector;
using System.Reflection;

namespace System.Collections.Generic
{
    public static partial class Extension
    {
        public static string GetEnumName(Enum e)
        {
            var type = e.GetType();
            var field = type.GetField(e.ToString());
            var attr = field.GetCustomAttribute<LabelTextAttribute>();
            return attr?.Text;
        }
    }
}

using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    [LabelText("比较方法")]
    public enum CompareType
    {
        [HideInInspector]
        None = 0,

        [LabelText("等于")]
        Equal = 1,

        [LabelText("不等于")]
        NotEqual = 2,

        [LabelText("大于")]
        GreaterThan = 3,

        [LabelText("大于等于")]
        GreaterThanOrEqual = 4,

        [LabelText("小于")]
        LessThan = 5,

        [LabelText("小于等于")]
        LessThanOrEqual = 6,
    }

    public partial class Helper
    {
        public static bool Compare(CompareType type, int value, int param)
        {
            return type switch
            {
                CompareType.Equal => value == param,
                CompareType.NotEqual => value != param,
                CompareType.GreaterThan => value > param,
                CompareType.GreaterThanOrEqual => value >= param,
                CompareType.LessThan => value < param,
                CompareType.LessThanOrEqual => value <= param,
                _ => false,
            };
        }

        public static bool Compare(CompareType type, float value, float param)
        {
            return type switch
            {
                CompareType.Equal => value == param,
                CompareType.NotEqual => value != param,
                CompareType.GreaterThan => value > param,
                CompareType.GreaterThanOrEqual => value >= param,
                CompareType.LessThan => value < param,
                CompareType.LessThanOrEqual => value <= param,
                _ => false,
            };
        }
    }
}

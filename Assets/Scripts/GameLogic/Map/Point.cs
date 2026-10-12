using UnityEngine;

namespace GameLogic
{
    public enum PointType
    {
        None = 0,
        起点 = 1,
        排队点 = 2, //[0,n]
        餐桌 = 3, //[1,n]
    }

    public class Point : MonoBehaviour
    {
        public PointType type;
        public int id;

        public bool Equal(PointType type, int id)
        {
            return this.type == type && this.id == id;
        }
    }
}

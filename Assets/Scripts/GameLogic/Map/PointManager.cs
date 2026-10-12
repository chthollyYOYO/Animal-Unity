using Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class PointManager : Singleton<PointManager>
    {
        private Dictionary<PointType, List<Point>> points = new();

        public void Initialize()
        {
            var connections = ConnectionManager.Instance.connections;
            foreach (var connection in connections)
            {
                void TryAddPoint(Point p)
                {
                    if (!points.TryGetValue(p.type, out var list))
                        list = points[p.type] = new();

                    if (!list.Contains(p))
                        list.Add(p);
                }

                TryAddPoint(connection.p1);
                TryAddPoint(connection.p2);
            }
        }

        public Point GetPointRandom(PointType type, List<Point> exclude = null)
        {
            if (points.TryGetValue(type, out var list))
            {
                List<Point> tmp = new();
                tmp.AddRange(list);
                if (exclude != null)
                {
                    for (int i = 0; i < exclude.Count; i++)
                        tmp.Remove(exclude[i]);
                }
                return tmp.RandomOneElement();
            }

            return null;
        }

        public Point GetPointRandom(PointType type, Point exclude)
        {
            return GetPointRandom(type, new List<Point> { exclude });
        }

        public Point GetPoint(PointType type)
        {
            if (points.TryGetValue(type, out var list))
            {
                return list[0];
            }
            return null;
        }

        public Point GetPoint(PointType type, int id)
        {
            if (points.TryGetValue(type, out var list))
            {
                return list.Find(it => it.id == id);
            }
            return null;
        }

        public int GetMaxId(PointType type)
        {
            if (points.TryGetValue(type, out var list))
            {
                return list.Max(it => it.id);
            }
            return 0;
        }
    }
}

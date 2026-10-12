using Framework;
using System.Collections.Generic;

namespace GameLogic
{
    public class PathFinderSystem : Singleton<PathFinderSystem>
    {
        public List<Point> FindPath(Point start, Point end)
        {
            // 使用DFS查找任意一条路径
            HashSet<Point> visited = new HashSet<Point>();
            List<Point> path = new List<Point>();

            if (DFS(start, end, visited, path))
            {
                return path;
            }

            return new();
        }

        /// <summary>
        /// 深度优先搜索查找路径
        /// </summary>
        private bool DFS(Point current, Point end, HashSet<Point> visited, List<Point> path)
        {
            // 标记当前点为已访问
            visited.Add(current);
            path.Add(current);

            // 找到终点
            if (current == end)
                return true;

            // 遍历所有邻居
            List<Point> neighbors = GetNeighbors(current);
            foreach (Point neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    if (DFS(neighbor, end, visited, path))
                        return true;
                }
            }

            // 回溯
            path.RemoveAt(path.Count - 1);
            return false;
        }

        /// <summary>
        /// 获取邻居节点
        /// </summary>
        private List<Point> GetNeighbors(Point point)
        {
            List<Point> neighbors = new();
            var connections = ConnectionManager.Instance.connections;
            foreach (var connection in connections)
            {
                if (connection.p1 == point)
                    neighbors.Add(connection.p2);
                else if (connection.p2 == point)
                    neighbors.Add(connection.p1);
            }
            return neighbors;
        }
    }
}

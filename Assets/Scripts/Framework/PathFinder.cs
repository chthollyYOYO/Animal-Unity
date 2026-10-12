using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class PathFinder
    {
        public class Node
        {
            // 当前节点位置
            public Vector2Int Position;
            // 父节点，用于回溯路径
            public Node Parent;
            // 从起点到此节点的代价
            public int G;
            // 从此节点到目的地的估计代价
            public int H;
            // 总代价
            public int F => G + H;

            private static Stack<Node> pool = new();

            private Node()
            {

            }

            public static Node Get()
            {
                if (pool.Count > 0)
                    return pool.Pop();
                else
                    return new();
            }

            public void Release()
            {
                Clear();
                pool.Push(this);
            }

            public void Clear()
            {
                Position = Vector2Int.zero;
                Parent = null;
                G = 0;
                H = 0;
            }
        }

        /// <summary>
        /// 该方法会返回一个以网格中心点的数据组，以作为精确寻路，注意，由于引用了MapManager，所以暂时只能在Runtime下调用
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <param name="walkableArea"></param>
        /// <returns></returns>
        public static List<Vector2> GetFinderPath(Vector2Int start, Vector2Int destination, HashSet<Vector2Int> walkableArea, Grid grid = null)
        {
            var path = GetPath(start, destination, walkableArea);
            List<Vector2> result = new();
            for (int i = 0; i < path.Count; i++)
                result.Add(new Vector2(path[i].x + 0.5f, path[i].y + 0.5f));
            return result;
        }

        /// <summary>
        /// 通过速度和持续时间计算插值
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Vector2 Interpolation(float speed, float time, List<Vector2> path)
        {
            var distance = speed * time;
            //如果参数中时间和速度乘积大于path的长度，则该插值为终点
            if (distance >= path.Count - 1)
                return path[path.Count - 1];

            var top = (int)Mathf.Ceil(distance);
            var down = (int)Mathf.Floor(distance);
            //如果正好在整数节点，则向前进一位
            if (top == down)
                top++;
            //小数点部分，即两个点之间的百分比。
            var fraction = distance - down;
            //确认方向
            var direction = path[top] - path[down];
            Vector2 added = direction * fraction;
            //Down 
            var result = path[down] + added;
            return result;
        }

        private static List<Vector2Int> GetPath(Vector2Int start, Vector2Int destination, HashSet<Vector2Int> walkableArea)
        {
            //开放列表，存放待检查的节点
            List<Node> openList = new List<Node>();
            //关闭列表，存放已检查的节点
            HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

            List<Node> alreadyCheckList = new();

            Node startNode = Node.Get();
            startNode.Position = start;
            startNode.G = 0;
            startNode.H = GetHeuristic(start, destination);
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                int minValue = int.MaxValue;
                Node currentNode = null;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].F < minValue)
                    {
                        minValue = openList[i].F;
                        currentNode = openList[i];
                    }
                }

                if (currentNode.Position == destination)
                {
                    var result = RetracePath(currentNode);
                    for (int i = 0; i < openList.Count; i++)
                        openList[i].Release();
                    for (int i = 0; i < alreadyCheckList.Count; i++)
                        alreadyCheckList[i].Release();
                    return result;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode.Position);

                var neighbors = GetNeighbors(currentNode.Position, walkableArea);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    Vector2Int neighbor = neighbors[i];
                    if (closedList.Contains(neighbor))
                        continue;

                    int tentativeGCost = currentNode.G + GetDistance(currentNode.Position, neighbor);

                    Node neighborNode = null;
                    for (int j = 0; j < openList.Count; j++)
                    {
                        if (openList[j].Position == neighbor)
                        {
                            neighborNode = openList[j];
                            break;
                        }
                    }

                    if (neighborNode == null)
                    {
                        neighborNode = Node.Get();
                        neighborNode.Position = neighbor;
                        openList.Add(neighborNode);
                    }

                    if (tentativeGCost < neighborNode.G || neighborNode.G == 0)
                    {
                        neighborNode.Parent = currentNode;
                        neighborNode.G = tentativeGCost;
                        neighborNode.H = GetHeuristic(neighbor, destination);
                    }
                }

                alreadyCheckList.Add(currentNode);
            }

            return new List<Vector2Int>(); // 如果找不到路径，返回空列表
        }

        private readonly static List<Vector2Int> _neighbors = new();
        private readonly static Vector2Int[] _directions = {
                new Vector2Int(1, 0), new Vector2Int(-1, 0),
                new Vector2Int(0, 1), new Vector2Int(0, -1)
            };
        //获取邻居节点
        private static List<Vector2Int> GetNeighbors(Vector2Int position, HashSet<Vector2Int> walkableArea)
        {
            _neighbors.Clear();
            for (int i = 0; i < _directions.Length; i++)
            {
                Vector2Int neighborPos = position + _directions[i];
                if (walkableArea.Contains(neighborPos))
                    _neighbors.Add(neighborPos);
            }

            return _neighbors;
        }

        //计算曼哈顿距离
        //曼哈顿距离‌，也称为城市街区距离或L1距离，用于标明两个点在标准坐标系上的绝对轴距总和。
        //即: d = |x1 - x2| + |y1 - y2|
        private static int GetHeuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        // 计算节点之间的实际距离
        private static int GetDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        // 回溯路径
        private static List<Vector2Int> RetracePath(Node endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node currentNode = endNode;

            while (currentNode != null)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}

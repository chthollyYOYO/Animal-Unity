using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    public static class GridConnecter
    {
        /// <summary>
        /// 通过位置信息构建网格，注意，如果要构建连接性较强的网格，请使用SplitIntoConnectedGroups方法将未连同的位置拆分为多个坐标信息组
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static Mesh CreateMeshFromPositions(List<Vector2Int> positions)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // 为每个坐标点创建一个1x1的矩形
            for (int i = 0; i < positions.Count; i++)
            {
                Vector2Int pos = positions[i];

                // 顶点坐标
                Vector3 bottomLeft = new Vector3(pos.x, pos.y, 0);
                Vector3 topLeft = new Vector3(pos.x, pos.y + 1, 0);
                Vector3 topRight = new Vector3(pos.x + 1, pos.y + 1, 0);
                Vector3 bottomRight = new Vector3(pos.x + 1, pos.y, 0);

                // 添加矩形的4个顶点
                vertices.Add(bottomLeft);
                vertices.Add(topLeft);
                vertices.Add(topRight);
                vertices.Add(bottomRight);

                // 添加两个三角形
                int startIndex = i * 4;
                triangles.Add(startIndex);
                triangles.Add(startIndex + 1);
                triangles.Add(startIndex + 2);

                triangles.Add(startIndex);
                triangles.Add(startIndex + 2);
                triangles.Add(startIndex + 3);
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();

            return mesh;
        }

        // 查找给定方向上的邻居（上、下、左、右）
        private static readonly Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // 右
            new Vector2Int(-1, 0),  // 左
            new Vector2Int(0, 1),   // 上
            new Vector2Int(0, -1)   // 下
        };

        /// <summary>
        /// 拆分成连接的位置信息组
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static List<List<Vector2Int>> SplitIntoConnectedGroups(List<Vector2Int> positions)
        {
            List<List<Vector2Int>> connectedGroups = new List<List<Vector2Int>>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            HashSet<Vector2Int> ps = positions.ToHashSet();

            foreach (var pos in positions)
            {
                // 如果坐标已经访问过，跳过
                if (visited.Contains(pos))
                    continue;

                // BFS 找到所有与当前坐标连通的块
                List<Vector2Int> connectedGroup = new List<Vector2Int>();
                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(pos);
                visited.Add(pos);

                while (queue.Count > 0)
                {
                    Vector2Int current = queue.Dequeue();
                    connectedGroup.Add(current);

                    // 检查四个方向的邻居
                    foreach (var direction in directions)
                    {
                        Vector2Int neighbor = current + direction;
                        if (ps.Contains(neighbor) && !visited.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);
                            visited.Add(neighbor);
                        }
                    }
                }

                // 将连通块添加到结果列表
                connectedGroups.Add(connectedGroup);
            }

            LogManager.Info($"格子连接，块数:{connectedGroups.Count}");
            return connectedGroups;
        }

        public static void PrintArray(int[,] array, int width, int height)
        {
            using (zstring.Block())
            {
                zstring str = string.Empty;
                for (int j = height - 1; j >= 0; j--)
                {
                    for (int i = 0; i < width; i++)
                        str += $" {array[i, j]}";
                    str += "\n";
                }
                Debug.Log(str);
            }
        }

        public static void PrintDictionary(Dictionary<int, int> dict)
        {
            using (zstring.Block())
            {
                zstring str = string.Empty;
                foreach (var item in dict)
                    str += $"{item.Key} -> {item.Value}\n";
                Debug.Log(str);
            }
        }
    }
}

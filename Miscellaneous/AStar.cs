using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    private struct Node
    {
        public Vector2Int position;
        public int gCost;
        public int hCost;
        public Vector2Int parent;
        public int fCost => gCost + hCost;

        public Node(int x, int y)
        {
            position = new Vector2Int(x, y);
            gCost = int.MaxValue;
            hCost = 0;
            parent = Vector2Int.zero;
        }
    }

    private static void GetNeighbours(Vector2Int[] neighbours, Vector2Int position)
    {
        neighbours[0] = position + Vector2Int.right;
        neighbours[1] = position + Vector2Int.up;
        neighbours[2] = position + Vector2Int.left;
        neighbours[3] = position + Vector2Int.down;
    }

    private static int GetDistance(Vector2Int pos1,Vector2Int pos2)
    {
        int dstX = Mathf.Abs(pos1.x - pos2.x);
        int dstY = Mathf.Abs(pos1.y - pos2.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public static Vector2Int[] FindShortestPath(Array2DBool maze, Vector2Int start, Vector2Int goal)
    {
        if (maze == null || maze.IsOutOfBounds(start.x, start.y) || maze.IsOutOfBounds(goal.x, goal.y) || !maze[start.x, start.y] || !maze[goal.x, goal.y])
        {
            return null;
        }
        IDictionary<Vector2Int, Node> openSet = new Dictionary<Vector2Int, Node>();
        IDictionary<Vector2Int, Node> closedSet = new Dictionary<Vector2Int, Node>();
        Vector2Int[] neighbours = new Vector2Int[4]; 

        openSet.Add(start, new Node(start.x, start.y));

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Aggregate((n1, n2) => (n1.Value.fCost < n2.Value.fCost || (n1.Value.fCost == n2.Value.fCost && n1.Value.hCost < n2.Value.hCost)) ? n1 : n2).Value;

            openSet.Remove(currentNode.position);
            closedSet.Add(currentNode.position, currentNode);

            if (currentNode.position == goal)
            {
                Stack<Vector2Int> path = new Stack<Vector2Int>();
                do
                {
                    path.Push(currentNode.position);
                    currentNode = closedSet[currentNode.parent];
                } while (currentNode.position != start);

                return path.ToArray();
            }

            GetNeighbours(neighbours, currentNode.position);
            for (int i = 0; i < neighbours.Length; ++i)
            {
                Vector2Int nPos = neighbours[i];
                if (maze.IsOutOfBounds(nPos.x, nPos.y) || !maze[nPos.x, nPos.y] || closedSet.ContainsKey(nPos))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode.position, nPos);
                Node nNode;
                bool neighbourVisited = openSet.ContainsKey(nPos);
                if (neighbourVisited)
                {
                    nNode = openSet[nPos];
                }
                else
                {
                    nNode = new Node(nPos.x, nPos.y);
                }
                if (newMovementCostToNeighbour < nNode.gCost || !neighbourVisited)
                {
                    nNode.gCost = newMovementCostToNeighbour;
                    nNode.hCost = GetDistance(nPos, goal);
                    nNode.parent = currentNode.position;

                    if (!neighbourVisited)
                    {
                        openSet.Add(nPos, nNode);
                    }
                }
            }

        }

        return null;
    }
}

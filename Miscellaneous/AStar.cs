using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolset
{
    public static class AStar
    {
        private class PriorityHeap<T> where T : IHeapItem<T>
        {
            List<T> _items;

            public PriorityHeap()
            {
                _items = new List<T>();
            }

            public bool Contains(T item) => Equals(_items[item.HeapIndex], item);

            public int Count => _items.Count;

            public void Add(T item)
            {
                item.HeapIndex = _items.Count;
                _items.Add(item);
                SortUp(item);
            }

            public T Pop()
            {
                T firstItem = _items.First();
                if (_items.Count > 1)
                {
                    _items[0] = _items.Last();
                    _items[0].HeapIndex = 0;
                    _items.RemoveAt(_items.Count - 1);
                    SortDown(_items[0]);
                }
                else
                {
                    _items.Clear();
                }
                return firstItem;

            }

            public void UpdateItem(T item)
            {
                SortUp(item);
            }

            private void SortDown(T item)
            {
                while (true)
                {
                    int childIndexLeft = item.HeapIndex * 2 + 1;
                    int childIndexRight = item.HeapIndex * 2 + 2;
                    int swapIndex;

                    if (childIndexLeft < _items.Count)
                    {
                        swapIndex = childIndexLeft;

                        if (childIndexRight < _items.Count)
                        {
                            if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                            {
                                swapIndex = childIndexRight;
                            }
                        }
                        if (item.CompareTo(_items[swapIndex]) < 0)
                        {
                            Swap(item, _items[swapIndex]);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            private void SortUp(T item)
            {
                int parentIndex = (item.HeapIndex - 1) / 2;

                while (true)
                {
                    T parentItem = _items[parentIndex];
                    if (item.CompareTo(parentItem) > 0)
                    {
                        Swap(parentItem, item);
                    }
                    else
                    {
                        break;
                    }

                    parentIndex = (item.HeapIndex - 1) / 2;
                }
            }

            private void Swap(T item1, T item2)
            {
                _items[item1.HeapIndex] = item2;
                _items[item2.HeapIndex] = item1;
                int item1Index = item1.HeapIndex;
                item1.HeapIndex = item2.HeapIndex;
                item2.HeapIndex = item1Index;
            }
        }

        private interface IHeapItem<T> : System.IComparable<T>
        {
            public int HeapIndex
            {
                get; set;
            }
        }

        private class OpenSet
        {
            private IDictionary<Vector2Int, Node> _positions;
            private PriorityHeap<Node> _costs;

            public int Count => _positions.Count;

            public Node this[Vector2Int position]
            {
                get => _positions[position];
            }

            public OpenSet()
            {
                _positions = new Dictionary<Vector2Int, Node>();
                _costs = new PriorityHeap<Node>();
            }

            public void Add(Node node)
            {
                _positions.Add(node.position, node);
                _costs.Add(node);
            }

            public Node Pop()
            {
                Node node = _costs.Pop();
                _positions.Remove(node.position);
                return node;
            }

            public bool Contains(Vector2Int position)
            {
                return _positions.ContainsKey(position);
            }

            public void UpdateNode(Node node)
            {
                _costs.UpdateItem(node);
            }
        }

        private class Node : IHeapItem<Node>
        {
            public Vector2Int position;
            public int gCost;
            public int hCost;
            public Vector2Int parent;
            public int fCost => gCost + hCost;

            public int HeapIndex { get; set; }

            public Node(int x, int y)
            {
                position = new Vector2Int(x, y);
                gCost = int.MaxValue;
                hCost = 0;
                parent = Vector2Int.zero;

                HeapIndex = -1;
            }

            public int CompareTo(Node other)
            {
                int compare = fCost.CompareTo(other.fCost);
                if (compare == 0)
                {
                    compare = hCost.CompareTo(other.hCost);
                }
                return -compare;
            }
        }

        private static void GetNeighbours(Vector2Int[] neighbours, Vector2Int position)
        {
            neighbours[0] = position + Vector2Int.right;
            neighbours[1] = position + Vector2Int.up;
            neighbours[2] = position + Vector2Int.left;
            neighbours[3] = position + Vector2Int.down;
        }

        private static int GetDistance(Vector2Int pos1, Vector2Int pos2)
        {
            int dstX = Mathf.Abs(pos1.x - pos2.x);
            int dstY = Mathf.Abs(pos1.y - pos2.y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }

        public static Vector2Int[] FindShortestPath(IArray2D<bool> maze, Vector2Int start, Vector2Int goal)
        {
            if (maze == null || maze.IsOutOfBounds(start.x, start.y) || maze.IsOutOfBounds(goal.x, goal.y) || !maze[start.x, start.y] || !maze[goal.x, goal.y])
            {
                return null;
            }
            OpenSet openSet = new OpenSet();
            IDictionary<Vector2Int, Node> closedSet = new Dictionary<Vector2Int, Node>();
            Vector2Int[] neighbours = new Vector2Int[4];

            openSet.Add(new Node(start.x, start.y));

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.Pop();
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
                    bool neighbourVisited = openSet.Contains(nPos);
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
                            openSet.Add(nNode);
                        }
                        else
                        {
                            openSet.UpdateNode(nNode);
                        }
                    }
                }

            }

            return null;
        }
    }
}

//Language Headers
using System.Collections.Generic;
//Engine Headers
using UnityEngine;

public class Pathfinding
{
    public class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public float GCost; // Cost from start to this node
        public float HCost; // Heuristic cost to the target
        public float FCost => GCost + HCost; // Total cost
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int target, bool[,] obstacles)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node { Position = start, GCost = 0, HCost = GetHeuristic(start, target) };
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);

            if (currentNode.Position == target)
            {
                return RetracePath(currentNode);
            }

            openList.Remove(currentNode);
            closedSet.Add(currentNode.Position);

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.Position, obstacles))
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeGCost = currentNode.GCost + 1;
                Node neighborNode = openList.Find(n => n.Position == neighbor);

                if (neighborNode == null)
                {
                    neighborNode = new Node
                    {
                        Position = neighbor,
                        Parent = currentNode,
                        GCost = tentativeGCost,
                        HCost = GetHeuristic(neighbor, target)
                    };
                    openList.Add(neighborNode);
                }
                else if (tentativeGCost < neighborNode.GCost)
                {
                    neighborNode.Parent = currentNode;
                    neighborNode.GCost = tentativeGCost;
                }
            }
        }

        return null; // No path found
    }

    private static Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowest = nodes[0];
        foreach (var node in nodes)
        {
            if (node.FCost < lowest.FCost)
                lowest = node;
        }
        return lowest;
    }

    private static float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan distance
    }

    private static List<Vector2Int> GetNeighbors(Vector2Int position, bool[,] obstacles)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = position + dir;
            if (neighbor.x >= 0 && neighbor.x < obstacles.GetLength(0) &&
                neighbor.y >= 0 && neighbor.y < obstacles.GetLength(1) &&
                !obstacles[neighbor.x, neighbor.y])
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

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
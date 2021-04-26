using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * A* Algorithm based on Code Monkey's implementation
 * 
 * Removed diagonal directions
 */

[System.Serializable]
public class Pathfinding
{
    public class PathNode
    {
        public int x;
        public int y;

        public int indexX;
        public int indexY;

        public bool wasEmpty;
        public bool isEmptyNode(Tilemap tm)
		{
            return !tm.HasTile(new Vector3Int(x, y, 0));
		}

        public int CostG;
        public int CostH;
        public int CostF => CostG + CostH;
        public PathNode prevNode;
    }

    private int xOffset;
    private int yOffset;

    private Tilemap tilemap;
    private PathNode[,] nodes;
    private int sizeX;
    private int sizeY;
    private PathNode GetNodeFromTilemapCoords(int x, int y) => nodes[x - xOffset, y - yOffset];


    public const int MOVE_COST = 10;

    public Pathfinding(Tilemap tiles)
    {
        tilemap = tiles;
        BoundsInt size = tiles.cellBounds;

        xOffset = size.xMin;
        yOffset = size.yMin;
        sizeX = size.xMax - xOffset;
        sizeY = size.yMax - yOffset;
        
        nodes = new PathNode[sizeX, sizeY];

        for (int j = 0; j < sizeY; j++)
        {
            for (int i = 0; i < sizeX; i++)
            {
                int x = i + xOffset;
                int y = j + yOffset;
                
                nodes[i, j] = new PathNode();
                PathNode node = nodes[i, j];
                node.indexX = i;
                node.indexY = j;
                node.x = x;
                node.y = y;
                node.wasEmpty = tiles.GetTile(new Vector3Int(x, y, 0)) == null;
            }
        }
    }

    public List<PathNode> FindPath(int x1, int y1, int x2, int y2)
    {
        PathNode start = GetNodeFromTilemapCoords(x1, y1);
        PathNode end = GetNodeFromTilemapCoords(x2, y2);

        List<PathNode> open = new List<PathNode> { start };
        List<PathNode> closed = new List<PathNode>();

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                PathNode node = nodes[i, j];

                node.CostG = int.MaxValue;

                node.prevNode = null;
            }
        }

        start.CostG = 0;
        start.CostH = CalcDistanceCost(start, end);

        while (open.Count > 0)
        {
            PathNode lowestF = LowestFCostNode(open);

            if (lowestF == end)
                return CalcPath(end);

            open.Remove(lowestF);
            closed.Add(lowestF);

            foreach (PathNode neighbor in GetNeightborList(lowestF))
            {
                if (closed.Contains(neighbor))
                    continue;

                if (!neighbor.isEmptyNode(tilemap))
                {
                    closed.Add(neighbor);
                    continue;
                }

                int tentativeCostG = lowestF.CostG + CalcDistanceCost(lowestF, neighbor);
                if (tentativeCostG < neighbor.CostG)
                {
                    neighbor.prevNode = lowestF;
                    neighbor.CostG = tentativeCostG;
                    neighbor.CostH = CalcDistanceCost(neighbor, end);

                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<PathNode> CalcPath(PathNode end)
    {
        List<PathNode> path = new List<PathNode>();

        while (end != null)
        {
            path.Add(end);
            end = end.prevNode;
        }

        path.Reverse();

        return path;
    }

    private List<PathNode> GetNeightborList(PathNode node)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if (node.indexX - 1 >= 0)
            neighborList.Add(nodes[node.indexX - 1, node.indexY]);
        if (node.indexX + 1 < sizeX)
            neighborList.Add(nodes[node.indexX + 1, node.indexY]);

        if (node.indexY - 1 >= 0)
            neighborList.Add(nodes[node.indexX, node.indexY - 1]);
        if (node.indexY + 1 < sizeY)
            neighborList.Add(nodes[node.indexX, node.indexY + 1]);

        return neighborList;
    }

    private int CalcDistanceCost(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);

        return MOVE_COST * (xDist + yDist);
    }

    private PathNode LowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestFCost = nodes[0];
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].CostF < lowestFCost.CostF)
                lowestFCost = nodes[i];
        }
        return lowestFCost;
    }
}

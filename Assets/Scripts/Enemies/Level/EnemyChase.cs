using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyChase : GridEnemyBase
{
    public Player player;
    public int lastPlayerX = int.MaxValue;
    public int lastPlayerY = int.MaxValue;

    public Tilemap map;

    private Pathfinding pathfinder;
    private List<Pathfinding.PathNode> path;

	public override void Kill()
	{
        // TODO
		throw new System.NotImplementedException("EnemyChase.Kill() not implemented");
	}

	// Start is called before the first frame update
	void Start()
    {
        SnapToGrid();

        pathfinder = new Pathfinding(map);
    }

    public override bool DoActions()
    {
        int playerX = player.GetX();
        int playerY = player.GetY();

        if (playerX != lastPlayerX || playerY != lastPlayerY)
        {
            lastPlayerX = playerX;
            lastPlayerY = playerY;

            path = pathfinder.FindPath(positionX, positionY, lastPlayerX, lastPlayerY);
            if (path == null)
                return true;

            path.RemoveAt(0); // Remove the starting node

            Debug.Log(path);
        }
        
        if (path == null)
            return true;

        if (path.Count > 0)
        {
            var node = path[0];

            int x = node.x - positionX;
            int y = node.y - positionY;

            Debug.Log("x = " + x + ", y = " + y);

            path.RemoveAt(0);

            MoveAlongGrid(node.x - positionX, node.y - positionY);
            
            return true;
        }
        return true;
    }
}

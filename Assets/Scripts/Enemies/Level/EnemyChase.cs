using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyChase : GridEnemyBase
{
    public int lastPlayerX = int.MaxValue;
    public int lastPlayerY = int.MaxValue;

    public override int DoActions(int actionsLeft)
    {
        int playerX = player.GetX();
        int playerY = player.GetY();

        if (playerX != lastPlayerX || playerY != lastPlayerY)
        {
            lastPlayerX = playerX;
            lastPlayerY = playerY;

            path = pathfinder.FindPath(position.x, position.y, lastPlayerX, lastPlayerY);
            if (path == null)
                return 1;

            path.RemoveAt(0); // Remove the starting node

            Debug.Log(path);
        }
        
        if (path == null)
            return 1;

        if (path.Count > 0)
        {
            var node = path[0];

            int x = node.x - position.x;
            int y = node.y - position.y;

            Debug.Log("x = " + x + ", y = " + y);

            path.RemoveAt(0);

            MoveAlongGrid(node.x - position.x, node.y - position.y);
            
            return 1;
        }
        return 1;
    }
}

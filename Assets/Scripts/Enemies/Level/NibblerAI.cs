using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * Nibbler will follow a set path
 * If a player comes near, it'll follow the player and try to bite the player
 * If the nibbler wonders to far following the player, it'll retreat back home
 */

public class NibblerAI : GridEnemyBase
{
    public Vector2Int[] targetPositions;
    private Vector2Int lastLocationOnPatrol; // This keeps track of the fish's last location before following the player

    private int nextPatrolPoint = 0;
    private int lastPatrolPoint = -1;

    private bool followingPlayer = false;

    [SerializeField]
    private float damageToDeal = 6.0f;

    private bool MadeItToTarget()
    {
        return playerPosAI == targetPositions[nextPatrolPoint];
    }

    public override int DoActions(int actionsLeft)
    {
        bool firstFollowIteration = false;

        if (PlayerDistance < 4.0f && !followingPlayer)
        {
            followingPlayer = true;
            firstFollowIteration = true;
            lastLocationOnPatrol = position;
        }

        if (followingPlayer)
        {
            if (PlayerDistance >= 4.0f)
            {
                path.Clear();
                followingPlayer = false;
                return 0;
            }

            if (lastPlayerPos != playerPos || firstFollowIteration)
            {
                Debug.Log("Pathfinding player...");
                lastPlayerPos = playerPos;

                path = pathfinder.FindPath(position.x, position.y, playerPos.x, playerPos.y);

#if UNITY_EDITOR
                if (path == null)
                {
                    Debug.LogError("Player is in unreachable position!!!!");
                    return 0;
                }
#endif

                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);
            }

            if (path.Count == 0)
            {
                player.Damage(damageToDeal);
                return 1;
            }
        }
        else
        {
            if (targetPositions.Length == 0)
                return 0;

            if (MadeItToTarget())
            {
                nextPatrolPoint++;

                if (nextPatrolPoint >= targetPositions.Length)
                    nextPatrolPoint = 0;
            }

            if (lastPatrolPoint != nextPatrolPoint || path.Count == 0)
            {
                Debug.Log("Pathfinding target...");
                lastPatrolPoint = nextPatrolPoint;

                Vector2Int nextPos = targetPositions[nextPatrolPoint] + turnBased.LevelTileOffset;

                path = pathfinder.FindPath(position.x, position.y, nextPos.x, nextPos.y);
#if UNITY_EDITOR
                if (path == null)
                {
                    Debug.LogError("This should not be null! Check your coordinates!");
                    Debug.Break();
                }
#endif
                path.RemoveAt(0);
            }
        }

        if (path.Count > 0)
        {
            var node = path[0];

            int x = node.x - position.x;
            int y = node.y - position.y;

            path.RemoveAt(0);

            MoveAlongGrid(node.x - position.x, node.y - position.y);
            return 1;
        }

        return 0;
    }
}

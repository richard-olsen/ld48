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
    public Tilemap tiles;

    public int nextPatrolPoint = 0;
    public int lastPatrolPoint = -1;

    public Vector2Int lastLocationOnPatrol; // This keeps track of the fish's last location before following the player

    public bool followingPlayer = false;

    public Player player;

    public Vector2Int playerPos;
    public Vector2Int lastPlayerPos;

    private List<Pathfinding.PathNode> path;
    private Pathfinding pathfinder;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinding(tiles);
        SnapToGrid();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePositions();
    }

    private bool MadeItToTarget()
    {
        return positionX == targetPositions[nextPatrolPoint].x && positionY == targetPositions[nextPatrolPoint].y;
    }

    public override bool DoActions()
    {
        playerPos.x = player.GetX();
        playerPos.y = player.GetY();

        Vector2Int currentPos = new Vector2Int(positionX, positionY);
        bool firstFollowIteration = false;

        float distanceFromPlayer = (playerPos - currentPos).magnitude;

        if (distanceFromPlayer < 4.0f && !followingPlayer)
        {
            followingPlayer = true;
            firstFollowIteration = true;
            lastLocationOnPatrol.x = positionX;
            lastLocationOnPatrol.y = positionY;
        }

        if (followingPlayer)
        {
            if (distanceFromPlayer >= 4.0f)
            {
                path.Clear();
                followingPlayer = false;
                return false;
            }

            if (lastPlayerPos != playerPos || firstFollowIteration)
            {
                lastPlayerPos = playerPos;

                path = pathfinder.FindPath(positionX, positionY, playerPos.x, playerPos.y);

#if UNITY_EDITOR
                if (path == null)
                {
                    Debug.LogError("Player is in unreachable position!!!!");
                    return false;
                }
#endif

                path.RemoveAt(0);
                path.RemoveAt(path.Count - 1);
            }

            if (path.Count == 0)
            {
                // TODO Attack Player
                return true;
            }
        }
        else
        {
            if (targetPositions.Length == 0)
                return false;

            if (MadeItToTarget())
            {
                nextPatrolPoint++;

                if (nextPatrolPoint >= targetPositions.Length)
                    nextPatrolPoint = 0;
            }

            if (lastPatrolPoint != nextPatrolPoint || path.Count == 0)
            {
                lastPatrolPoint = nextPatrolPoint;

                Vector2Int nextPos = targetPositions[nextPatrolPoint];

                path = pathfinder.FindPath(positionX, positionY, nextPos.x, nextPos.y);
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

            int x = node.x - positionX;
            int y = node.y - positionY;

            path.RemoveAt(0);

            MoveAlongGrid(node.x - positionX, node.y - positionY);
            return true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public abstract class GridEnemyBase : GridAlignedEntity
{
    protected Player player;
    [Tooltip("The Tilemap of the level for pathfinding")]
    public Tilemap map;

    protected Pathfinding pathfinder;
    protected List<Pathfinding.PathNode> path;

    protected Vector2Int playerPos;
    protected Vector2Int lastPlayerPos;

    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        pathfinder = new Pathfinding(map);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerObject, "A player must exist in the scene!");

        GameObject turnBaseController = GameObject.FindGameObjectWithTag("TurnBaseController");
        Assert.IsNotNull(turnBaseController, "A turn based controller must exist in the scene!");

        player = playerObject.GetComponent<Player>();
        Assert.IsNotNull(player, "Player GameObject REQUIRES the Player class! Are you using the prefab?");

        TurnBasedMovementSystem turnbased = turnBaseController.GetComponent<TurnBasedMovementSystem>();
        Assert.IsNotNull(turnbased, "TurnBasedController REQUIRES the class! Are you using the prefab?");

        turnbased.AddEnemy(this);

        SnapToGrid();
    }

    protected void FixedUpdate()
    {
        playerPos.x = player.GetX();
        playerPos.y = player.GetY();

        UpdatePositions();
    }

    // Update is called once per frame
    protected void Update()
    {
        Vector3 animationPosition = targetPosition - transform.position;

        // Worry about X first, then Y

        if (animationPosition.x < -float.Epsilon)
        {
            animator.SetFloat("animX", -1);
        }
        else if (animationPosition.x > float.Epsilon)
        {
            animator.SetFloat("animX", 1);
        }
        else
        {
            animator.SetFloat("animX", 0);
        }

        if (animationPosition.y < -float.Epsilon)
        {
            animator.SetFloat("animY", -1);
        }
        else if (animationPosition.y > float.Epsilon)
        {
            animator.SetFloat("animY", 1);
        }
        else
        {
            animator.SetFloat("animY", 0);
        }
    }

    // Constantly called. Action decisions are made here
    public abstract bool DoActions();
}

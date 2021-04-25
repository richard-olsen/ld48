using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
public abstract class GridEnemyBase : GridAlignedEntity
{
    public Player player;
    public Tilemap map;

    protected Pathfinding pathfinder;
    protected List<Pathfinding.PathNode> path;

    protected Vector2Int playerPos;
    protected Vector2Int lastPlayerPos;

    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SnapToGrid();
        animator = GetComponent<Animator>();
        pathfinder = new Pathfinding(map);
    }

    private void FixedUpdate()
    {
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

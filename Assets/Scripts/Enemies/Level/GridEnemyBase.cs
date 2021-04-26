using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public abstract class GridEnemyBase : GridAlignedEntity, IDamageable
{
    protected Player player;
    [Tooltip("The Tilemap of the level for pathfinding")]
    public Tilemap map;

    protected Pathfinding pathfinder;
    protected List<Pathfinding.PathNode> path;

    protected Vector2Int playerPos;
    protected Vector2Int lastPlayerPos;

    protected Animator animator;

    private TurnBasedMovementSystem turnBased;

    // can only be damaged while _canBeDamaged is true
    protected bool _canBeDamaged = true;
    public bool CanBeDamaged => _canBeDamaged;

    protected float PlayerDistance => (position - playerPos).magnitude;

    [SerializeField]
    protected float _maxHealth = 10f;
    public float MaxHealth => _maxHealth;

    /// <summary>
    /// how much health the character has left before they die
    /// </summary>
    public float Health => _health;
    protected float _health;

    /// <summary>
    /// whether or not the enemy is currently alive
    /// </summary>
    public bool IsAlive => _health > 0;

    public virtual void KnockBack(Vector2Int kb)
	{
        if (!MoveAlongGrid(kb.x, 0)) 
            MoveAlongGrid(0, kb.y);
	}

    /// <summary>
    /// causes the enemy to take damage
    /// </summary>
    /// <param name="damage">the amount of damage that the enemy should take</param>
    public virtual void Damage(float damage)
	{
        _health -= damage;
		if (!IsAlive)
		{
            Kill();
		}
	}

    /// <summary>
    /// Kills the enemy
    /// </summary>
    public virtual void Kill()
    {
        turnBased.RemoveEnemy(this, true);
        Destroy(gameObject);
    }

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

        turnBased = turnBaseController.GetComponent<TurnBasedMovementSystem>();
        Assert.IsNotNull(turnBased, "TurnBasedController REQUIRES the class! Are you using the prefab?");

        turnBased.AddEnemy(this);

        _health = _maxHealth;

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
    public abstract int DoActions(int actionsLeft);
}

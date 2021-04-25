using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class GridEnemyBase : GridAlignedEntity, IDamageable
{
    protected Animator animator;

    // can only be damaged while _canBeDamaged is true
    protected bool _canBeDamaged = true;
    public bool CanBeDamaged => _canBeDamaged;

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
    public abstract void Kill();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // set the enemy health to it's max health at start
        _health = _maxHealth;

        SnapToGrid();
        animator = GetComponent<Animator>();
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

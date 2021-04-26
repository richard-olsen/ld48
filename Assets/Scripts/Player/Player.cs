using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : GridAlignedEntity, IDamageable
{
    public float Health => oxygenLevel;
    public float MaxHealth => maxOxygenLevel;

    public bool CanBeDamaged => true;
    public bool IsAlive => Health > 0;

    [SerializeField]
    private GameObject _hitPrefab;

    // I'll leave these as integers for now
    // no.
    [SerializeField]
    private float maxOxygenLevel = 25;
    private float oxygenLevel; // is set in Start()

    [SerializeField]
    private float _meleeDamage = 5;
    public float MeleeDamage => _meleeDamage;

    private Vector2Int _prevMove;
    public Vector2Int PreviousMoveDirection => _prevMove;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private float movementOxygenCost = 0.5f;

    public bool usingMenus;

    // Start is called before the first frame update
    protected void Start()
    {
        SnapToGrid();
        oxygenLevel = maxOxygenLevel;
    }

    protected void FixedUpdate()
    {
        UpdatePositions();
    }

    public int DoActions(int actionsLeft)
    {
        if (oxygenLevel <= 0)
            return 0;

        if (!usingMenus)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            int moveX = 0;
            int moveY = 0;

            if (hor < 0)
                moveX--;
            if (hor > 0)
                moveX++;

            if (ver < 0)
                moveY--;
            if (ver > 0)
                moveY++;

            if (MoveAlongGrid(moveX, moveY))
            {
                _prevMove = new Vector2Int(moveX, moveY);
                DepleteOxygen(movementOxygenCost);
                return 1;
            }
        }

        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 animationPosition = targetPosition - transform.position;

        // Worry about X first, then Y

        if (animationPosition.x < -float.Epsilon)
        {
            playerAnimator.SetFloat("animX", -1);
        }
        else if (animationPosition.x > float.Epsilon)
        {
            playerAnimator.SetFloat("animX", 1);
        }
        else
        {
            playerAnimator.SetFloat("animX", 0);
        }

        if (animationPosition.y < -float.Epsilon)
        {
            playerAnimator.SetFloat("animY", -1);
        }
        else if (animationPosition.y > float.Epsilon)
        {
            playerAnimator.SetFloat("animY", 1);
        }
        else
        {
            playerAnimator.SetFloat("animY", 0);
        }
    }

    public void KnockBack(Vector2Int kb)
	{
        if (!MoveAlongGrid(kb.x, 0))
            MoveAlongGrid(0, kb.y);
	}

    public void Damage(float damage)
	{
        if (damage < 0)
            GiveOxygen(-damage);
        else
            DepleteOxygen(damage);

        Instantiate(_hitPrefab, transform.position + Vector3.back, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        HUDController.Noise_Hit();

        // kill if health/oxygen goes to or below 0
        if (!IsAlive)
            Kill();
	}

    public void Kill()
	{
        IEnumerator gameOverScreen()
        {
            yield return new WaitForSeconds(1.5f);


            // Make sure the player is still dead
            // Even though the player can't move, there could
            // be a case where the player got an air bubble
            // at the exact moment they hit 0 oxygen
            // Just to be nice to the player :)
            if (!IsAlive)
                SceneManager.LoadScene("Scenes/GameOver");
        }
        StartCoroutine(gameOverScreen());
	}

    public void GiveOxygen(float oxygen)
    {
        oxygenLevel += oxygen;

        if (oxygenLevel >= maxOxygenLevel)
            oxygenLevel = maxOxygenLevel;
    }

    public void DepleteOxygen(float oxygen)
    {
        oxygenLevel -= oxygen;

        if (oxygenLevel < 0)
            oxygenLevel = 0;
    }

    public float GetOxygenLevel()
    {
        return oxygenLevel;
    }

    public float GetMaxOxygenLevel()
    {
        return maxOxygenLevel;
    }
}

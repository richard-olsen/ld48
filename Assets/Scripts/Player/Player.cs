using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GridAlignedEntity
{
    // I'll leave these as integers for now
    [SerializeField]
    private int oxygenLevel = 10;
    private int maxOxygenLevel = 10;

    [SerializeField]
    private Animator playerAnimator;

    public bool usingMenus;

    // Start is called before the first frame update
    protected void Start()
    {
        SnapToGrid();
    }

    protected void FixedUpdate()
    {
        UpdatePositions();
    }

    public int DoActions(int actionsLeft)
    {
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

    public void GiveOxygen(int oxygen)
    {
        oxygenLevel += oxygen;

        if (oxygenLevel >= maxOxygenLevel)
            oxygenLevel = maxOxygenLevel;
    }

    public void DepleteOxygen(int oxygen)
    {
        oxygenLevel -= oxygen;

        if (oxygenLevel < 0)
            oxygenLevel = 0;
    }

    public int GetOxygenLevel()
    {
        return oxygenLevel;
    }

    public int GetMaxOxygenLevel()
    {
        return maxOxygenLevel;
    }
}

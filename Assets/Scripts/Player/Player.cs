using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GridAlignedEntity
{
    [SerializeField]
    private int oxygenLevel = 10;
    private int maxOxygenLevel = 10;

    private bool canMoveX = true;
    private bool canMoveY = true;

    // Start is called before the first frame update
    void Start()
    {
        SnapToGrid();
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        int moveX = 0;
        int moveY = 0;

        if (canMoveX)
        {
            if (hor < 0)
                moveX--;
            if (hor > 0)
                moveX++;

            canMoveX = false;
        }
        if (canMoveY)
        {
            if (ver < 0)
                moveY--;
            if (ver > 0)
                moveY++;

            canMoveY = false;
        }

        if (hor == 0)
            canMoveX = true;
        if (ver == 0)
            canMoveY = true;

        if (moveX != 0 || moveY != 0)
            MoveAlongGrid(moveX, moveY);
    }

    // Update is called once per frame
    void Update()
    {
        if (oxygenLevel > maxOxygenLevel)
            oxygenLevel = maxOxygenLevel;
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

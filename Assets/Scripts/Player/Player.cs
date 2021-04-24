using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GridAlignedEntity
{
    // I'll leave these as integers for now
    [SerializeField]
    private int oxygenLevel = 10;
    private int maxOxygenLevel = 10;

    private float oxygenLoseTimer = 0;
    private float timeTillLoseOxygen = 3.0f; // Ten seconds until player loses 1 oxygen point

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

        MoveAlongGrid(moveX, moveY);
    }

    // Update is called once per frame
    void Update()
    {

        oxygenLoseTimer += Time.deltaTime;
        if (oxygenLoseTimer >= timeTillLoseOxygen)
        {
            oxygenLoseTimer = 0;
            oxygenLevel -= 1;
        }


        if (oxygenLevel > maxOxygenLevel)
            oxygenLevel = maxOxygenLevel;

        // Trigger event if oxygen level <= 0
    }

    public void GiveOxygen(int oxygen)
    {
        oxygenLevel += oxygen;

        if (oxygenLevel >= maxOxygenLevel)
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

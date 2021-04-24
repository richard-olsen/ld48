using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolSimple : GridEnemyBase
{
    [SerializeField]
    private int positionX1;

    [SerializeField]
    private int positionX2;

    private float decideTime = 0;
    private bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        SnapToGrid();
    }

    private void FixedUpdate()
    {
        if (decideTime >= 1.0f)
        {
            decideTime = 0;

            if (positionX <= Mathf.Min(positionX1, positionX2))
                canMove = true;
            else if (positionX >= Mathf.Max(positionX1, positionX2))
                canMove = false;

            
            MoveAlongGrid(canMove ? 1 : -1, 0);
        }

        decideTime += Time.deltaTime;

        UpdatePositions();
    }
}

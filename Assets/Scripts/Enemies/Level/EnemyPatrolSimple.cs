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

	public override void Kill()
	{
        // TODO
		throw new System.NotImplementedException("EnemyPatrolSimple.Kill() no implemented");
	}

	// Start is called before the first frame update
	void Start()
    {
        SnapToGrid();
    }

    private void FixedUpdate()
    {
        UpdatePositions();
    }

    public override bool DoActions()
    {
        if (positionX <= Mathf.Min(positionX1, positionX2))
            canMove = true;
        else if (positionX >= Mathf.Max(positionX1, positionX2))
            canMove = false;

        if (MoveAlongGrid(canMove ? 1 : -1, 0))
            return true;

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlignedEntity : MonoBehaviour
{
    protected Vector2Int position;
    protected Vector3 targetPosition;
    private Vector3 oldPosition;

    protected static float interpolateLength = 0.2f;
    public static float InterpolateLength => interpolateLength;

    protected float interpolateTime = 1.0f;

    protected void UpdatePositions()
    {
        if (interpolateTime >= interpolateLength)
        {
            oldPosition = targetPosition;
            transform.position = targetPosition;
            return;
        }

        transform.position = Vector3.Lerp(oldPosition, targetPosition, interpolateTime / interpolateLength);

        interpolateTime += Time.deltaTime;
    }

    public void SnapToGrid()
    {
        int x = Mathf.RoundToInt(transform.position.x - 0.5f);
        int y = Mathf.RoundToInt(transform.position.y - 0.5f);

        SetAlongX(x);
        SetAlongY(y);
    }

    public void MoveAlongX(int xOffset)
    {
        position.x += xOffset;

        targetPosition.x = (float)position.x + 0.5f;

        interpolateTime = 0;
    }

    public void MoveAlongY(int yOffset)
    {
        position.y += yOffset;

        targetPosition.y = (float)position.y + 0.5f;

        interpolateTime = 0;
    }

    public void SetAlongX(int x)
    {
        position.x = x;

        targetPosition.x = (float)position.x + 0.5f;

        oldPosition = targetPosition;
        transform.position = targetPosition;

        interpolateTime = 1.0f;
    }

    public void SetAlongY(int y)
    {
        position.y = y;

        targetPosition.y = (float)position.y + 0.5f;

        oldPosition = targetPosition;
        transform.position = targetPosition;

        interpolateTime = 1.0f;
    }

    // Returns true if movement is successful
    public bool MoveAlongGrid(int xOffset, int yOffset)
    {
        if (xOffset == 0 && yOffset == 0)
            return false;
        if (interpolateTime < interpolateLength)
            return false;

        if (xOffset != 0)
            yOffset = 0;

        // Do some ray casting to see if entity can move into slot
        // Try to move to another slot if possible
        // Assuming everything is a bounding box filling the entire single grid space
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(targetPosition.x, targetPosition.y), new Vector2(xOffset, yOffset).normalized, 1.0f, LayerMask.GetMask("Default"));

        bool solidCol = false;
        foreach(RaycastHit2D rch in hits)
		{
            if (rch.collider.isTrigger)
                continue;
            solidCol = true;
		}

        if (solidCol) // Has collision
        {
            return false;
        }

        oldPosition = targetPosition;
        MoveAlongX(xOffset);
        MoveAlongY(yOffset);
        return true;
    }

    public int GetX()
    {
        return position.x;
    }
    public int GetY()
    {
        return position.y;
    }
}

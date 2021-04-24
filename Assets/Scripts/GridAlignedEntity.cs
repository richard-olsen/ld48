using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlignedEntity : MonoBehaviour
{
    protected int positionX;
    protected int positionY;
    protected BoxCollider2D collider;
    protected Vector3 targetPosition;
    private Vector3 oldPosition;

    protected float interpolateTime = 1.0f;

    protected void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    protected void UpdatePositions()
    {
        if (interpolateTime >= 1.0f)
        {
            oldPosition = targetPosition;
            transform.position = targetPosition;
            return;
        }

        transform.position = Vector3.Lerp(oldPosition, targetPosition, interpolateTime);

        interpolateTime += Time.deltaTime * 3.0f;
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
        positionX += xOffset;

        targetPosition.x = (float)positionX + 0.5f;

        interpolateTime = 0;
    }

    public void MoveAlongY(int yOffset)
    {
        positionY += yOffset;

        targetPosition.y = (float)positionY + 0.5f;

        interpolateTime = 0;
    }

    public void SetAlongX(int x)
    {
        positionX = x;

        targetPosition.x = (float)positionX + 0.5f;

        oldPosition = targetPosition;
        transform.position = targetPosition;

        interpolateTime = 1.0f;
    }

    public void SetAlongY(int y)
    {
        positionY = y;

        targetPosition.y = (float)positionY + 0.5f;

        oldPosition = targetPosition;
        transform.position = targetPosition;

        interpolateTime = 1.0f;
    }

    // Returns true if movement is successful
    public bool MoveAlongGrid(int xOffset, int yOffset)
    {
        if (xOffset == 0 && yOffset == 0)
            return false;
        if (interpolateTime < 1.0f)
            return false;

        if (xOffset != 0)
            yOffset = 0;

        // Do some ray casting to see if entity can move into slot
        // Try to move to another slot if possible
        // Assuming everything is a bounding box filling the entire single grid space
        RaycastHit2D rays = Physics2D.Raycast(new Vector2(targetPosition.x, targetPosition.y), new Vector2(xOffset, yOffset).normalized, 0.5f, LayerMask.GetMask("Default"));

        if (rays.collider != null && !rays.collider.isTrigger) // Has collision
        {
            return false;
        }

        oldPosition = targetPosition;
        MoveAlongX(xOffset);
        MoveAlongY(yOffset);
        return true;
    }
}

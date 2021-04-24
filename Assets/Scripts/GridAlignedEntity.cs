using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlignedEntity : MonoBehaviour
{
    protected int positionX;
    protected int positionY;

    public void SnapToGrid()
    {
        int x = Mathf.RoundToInt(transform.position.x - 0.5f);
        int y = Mathf.RoundToInt(transform.position.y - 0.5f);

        MoveAlongX(x);
        MoveAlongY(y);
    }

    public void MoveAlongX(int xOffset)
    {
        Vector3 position = transform.position;

        positionX += xOffset;

        position.x = (float)positionX + 0.5f;

        transform.position = position;
    }

    public void MoveAlongY(int yOffset)
    {
        Vector3 position = transform.position;

        positionY += yOffset;

        position.y = (float)positionY + 0.5f;

        transform.position = position;
    }

    // Returns true if movement is successful
    public bool MoveAlongGrid(int xOffset, int yOffset)
    {
        if (xOffset == 0 && yOffset == 0)
            return false;

        // Do some ray casting to see if entity can move into slot
        // Try to move to another slot if possible
        // Assuming everything is a bounding box filling the entire single grid space
        RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(xOffset, yOffset).normalized, 1.0f);

        if (ray.collider != null) // Something exists in the targeted slot
        {
            // Try along 2 other axis

            if (xOffset == 0 || yOffset == 0)
                return false; // No where else to try

            if (MoveAlongGrid(xOffset, 0))
                return true;
            if (MoveAlongGrid(0, yOffset))
                return true;

            return false; // No where else to try
        }

        MoveAlongX(xOffset);
        MoveAlongY(yOffset);

        return true;
    }
}

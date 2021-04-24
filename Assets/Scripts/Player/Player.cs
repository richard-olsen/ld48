using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int positionX = 0;
    private int prevPositionX = 0;
    
    [SerializeField]
    private int positionY = 0;
    private int prevPositionY = 0;

    [SerializeField]
    private bool canMoveX = true;
    [SerializeField]
    private bool canMoveY = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if (canMoveX)
        {
            if (hor < 0)
                positionX--;
            if (hor > 0)
                positionX++;

            canMoveX = false;
        }
        if (canMoveY)
        {
            if (ver < 0)
                positionY--;
            if (ver > 0)
                positionY++;

            canMoveY = false;
        }

        if (hor == 0)
            canMoveX = true;
        if (ver == 0)
            canMoveY = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool changed = false;

        if (positionX != prevPositionX)
            changed = true;

        if (positionY != prevPositionY)
            changed = true;

        if (changed)
        {
            Vector3 position = transform.position;

            position.x = (float)positionX + 0.5f;
            position.y = (float)positionY + 0.5f;

            prevPositionX = positionX;
            prevPositionY = positionY;

            transform.position = position;
        }
    }
}

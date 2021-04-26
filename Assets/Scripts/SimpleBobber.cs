using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBobber : MonoBehaviour
{
    public Vector2 scale;
    public Vector2 speed;
    public Vector2 offset;
    private float time = 0;
    private Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos.x = transform.localPosition.x;
        pos.y = transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        Vector3 position = transform.localPosition;
        position.x = pos.x + Mathf.Sin(time * speed.x + offset.x) * scale.x;
        position.y = pos.y + Mathf.Sin(time * speed.y + offset.y) * scale.y;
        transform.localPosition = position;
    }
}

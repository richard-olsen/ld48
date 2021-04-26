using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBounce : MonoBehaviour
{
    public float scale;
    public float speed;
    private float time = 0;
    private float y = 0;
    // Start is called before the first frame update
    void Start()
    {
        y = transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;

        Vector3 pos = transform.localPosition;
        pos.y = y + Mathf.Sin(time * speed) * scale;
        transform.localPosition = pos;
    }
}

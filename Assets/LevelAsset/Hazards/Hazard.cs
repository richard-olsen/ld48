using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [HideInInspector]
    public bool DisableWhenOutOfLevel = false;

    private void handleDisableCheck()
	{
        if (!DisableWhenOutOfLevel)
            return;

        Vector3 min = Camera.main.ViewportToWorldPoint(Camera.main.rect.min);
        Vector3 max = Camera.main.ViewportToWorldPoint(Camera.main.rect.max);

        // get the camera viewport in world coords and expand it by one in each direction
        Rect vp = new Rect(min, max - min);
        vp.position -= new Vector2(2, 2);
        vp.size += new Vector2(1, 1);

        // if the object falls outside the camera viewport
        if (!vp.Contains(transform.position))
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleDisableCheck();
    }
}

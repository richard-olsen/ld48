using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidyAI : GridEnemyBase
{
	public override void Kill()
	{
        // TODO
		throw new System.NotImplementedException("SquidyAI.Kill() not implemented");
	}

	[SerializeField]
    private GameObject circle;
    IEnumerator CircleVisual()
    {
        circle.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        circle.SetActive(false);
    }
    public override int DoActions(int actionsLeft)
    {
        if (PlayerDistance <= 5.0f && actionsLeft >= 2)
        {
            StartCoroutine(CircleVisual());

            player.DepleteOxygen(3);
            return 2;
        }

        return 1;
    }
}

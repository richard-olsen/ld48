using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidyAI : GridEnemyBase
{
    [SerializeField]
    private GameObject circle;
    [SerializeField]
    private float radius;

    [SerializeField]
    private float damageToDeal = 7.0f;

    protected override void Start()
    {
        base.Start();

        Vector3 scale = circle.transform.localScale;
        scale.x = radius * 2f;
        scale.y = radius * 2f;
        circle.transform.localScale = scale;
    }

    IEnumerator CircleVisual()
    {
        circle.SetActive(true);
        yield return new WaitForSeconds(GridAlignedEntity.interpolateLength * 3 + 0.1f);
        circle.SetActive(false);

    }

    public override int DoActions(int actionsLeft)
    {
        if (PlayerDistance <= radius && actionsLeft >= 2)
        {
            StartCoroutine(CircleVisual());

            player.Damage(damageToDeal);
            return 2;
        }

        return 1;
    }
}

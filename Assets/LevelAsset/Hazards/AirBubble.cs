using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBubble : MonoBehaviour, IDamageable, IInteractible
{
	[SerializeField]
	private bool _floatUpward = true;

	[SerializeField]
	private float _oxygenAmount = 12.5f;

	private GridSnap _gridSnap;
	public GridSnap gridSnap => _gridSnap ?? (_gridSnap = GetComponent<GridSnap>());

	private float _health = 1;
	public float Health => _health;
	public float MaxHealth => 1;
	public bool CanBeDamaged => true;
	public bool IsAlive => _health > 0;
	
	public void InteractWith(IInteractor interactor)
	{
		PlayerInteraction plrInter = interactor.gameObject.GetComponent<PlayerInteraction>();
		if (plrInter != null)
		{
			plrInter.SelectInteractible(this);
		}
	}

	public void Damage(float dmg)
	{
		Kill();
	}

	public void Kill() 
	{
		Destroy(gameObject);
	}

	private void handleFloatUp()
	{
		// float upwards if possible
		if (gridSnap.IsSnapped)
		{
			bool canMoveUp = true;
			foreach (Collider2D col in gridSnap.CollisionsAtOffset(Vector3Int.up))
			{
				if (col.GetComponent<Player>() != null)
					continue;
				canMoveUp = false;
				break;
			}

			// float upward
			if(canMoveUp)
				gridSnap.MoveCells(Vector3Int.up);
		}
	}

	private void Update()
	{
		handleFloatUp();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if the collision is with the player
		Player plr = collision.gameObject.GetComponent<Player>();
		if(plr != null)
		{
			plr.GiveOxygen(_oxygenAmount);
			Kill();
		}
	}
}

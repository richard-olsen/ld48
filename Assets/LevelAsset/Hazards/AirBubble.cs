using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBubble : MonoBehaviour, IDamageable
{
	[SerializeField]
	private bool _floatUpward = true;

	[SerializeField]
	private float _oxygenAmount = 12.5f;

	private float _health = 1;
	public float Health => _health;
	public float MaxHealth => 1;
	public bool CanBeDamaged => true;
	public bool IsAlive => _health > 0;

	public void Damage(float dmg)
	{
		Kill();
	}

	public void Kill() 
	{
		Destroy(gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameObjectable
{
	public GameObject gameObject { get; }
	public Transform transform { get; }
}

public interface IInteractible : IGameObjectable
{
	public void InteractWith(IInteractor interactor);
}

public interface IInteractor : IGameObjectable
{

}

public interface IDamageable : IGameObjectable 
{
	public bool CanBeDamaged { get; }
	public bool IsAlive { get; }

	public float Health { get;  }
	public float MaxHealth { get; }

	public void KnockBack(Vector2Int kb);
	public void Damage(float damage);
	public void Kill();
}
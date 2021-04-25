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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelAssetController : MonoBehaviour
{
	[SerializeField]
	private Tilemap _tilemap;
	public Tilemap Tilemap => _tilemap;

	public TurnBasedMovementSystem turnSystem;

	public void OnLevelLoad()
	{

	}

	public void OnLevelUnload()
	{

	}
}

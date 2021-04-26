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

	private Vector2Int positionOffset;
	public Vector2Int LevelTileOffset => positionOffset;

	public void OnLevelLoad()
	{

	}

	public void OnLevelUnload()
	{

	}

    private void FixedUpdate()
    {
		// Not the best idea, but time is limited
		positionOffset.x = Mathf.RoundToInt(transform.position.x);
		positionOffset.y = Mathf.RoundToInt(transform.position.y);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingRock : MonoBehaviour, IInteractible
{
	private LevelAssetController _parentLevel;
	public LevelAssetController ParentLevel => _parentLevel ?? (_parentLevel = transform.GetComponentInParent<LevelAssetController>());

	protected Vector3Int cellPosition => _gridsnap.GetCellPosition();
	protected Tilemap tilemap => ParentLevel.Tilemap;

	private bool _isFalling = false;
	public bool IsFalling => _isFalling;

	public void InteractWith(IInteractor interactor)
	{
		// TODO
	}

	[SerializeField]
	private GridSnap _gridsnap;
	[SerializeField]
	private float _fallTickLength = 0.2f;

	private void fallCheck()
	{
		if (!tilemap.HasTile(cellPosition + Vector3Int.down))
		{
			_gridsnap.MoveCells(Vector3Int.down, _fallTickLength);
			_isFalling = true;
		}
		else
		{
			_isFalling = false;
		}
	}

	private void Update()
	{
		if (_gridsnap.IsSnapped)
		{
			fallCheck();
		}
	}
}

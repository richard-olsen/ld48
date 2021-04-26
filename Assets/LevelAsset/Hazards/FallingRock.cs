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

	protected Tile _tile;
	protected Tile tile => _tile ?? (_tile = ScriptableObject.CreateInstance<Tile>());

	private bool _isFalling = false;
	public bool IsFalling => _isFalling;

	public void InteractWith(IInteractor interactor)
	{
		PlayerInteraction plrInter = interactor.gameObject.GetComponent<PlayerInteraction>();
		if(plrInter != null)
		{
			plrInter.SelectInteractible(this);
		}
	}

	[SerializeField]
	private GridSnap _gridsnap;
	[SerializeField]
	private float _fallTickLength = 0.2f;

	private void fallCheck()
	{
		bool canFall = true;
		foreach(Collider2D col in _gridsnap.CollisionsAtOffset(Vector3Int.down))
		{
			if (col.isTrigger)
				continue;
			if (col.GetComponent<IDamageable>() != null)
				continue;
			canFall = false;
		}
		if (canFall)
		{
			_gridsnap.MoveCells(Vector3Int.down, _fallTickLength);
			_isFalling = true;
			tilemap.SetTile(_gridsnap.GetCellPosition(), null);
		}
		else
		{
			_isFalling = false;
			if(!tilemap.HasTile(_gridsnap.GetCellPosition()))
				tilemap.SetTile(_gridsnap.GetCellPosition(), tile);
		}
	}

	private void Update()
	{
		if (_gridsnap.IsSnapped)
		{
			fallCheck();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsFalling)
		{
			IDamageable dam = collision.gameObject.GetComponent<IDamageable>();
			if(dam != null && dam.CanBeDamaged)
			{
				Debug.Log("Falling rock hits " + dam + "for 10 dmg");
				dam.Damage(10);
			}
		}
	}
}

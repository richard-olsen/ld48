using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridSnap : MonoBehaviour
{
	public static readonly float maxCellDistance = 0.01f;

	private LevelAssetController _parentLevel;
	public LevelAssetController ParentLevel {
		get => (_parentLevel == null || !_parentLevel.gameObject.activeInHierarchy) ? (_parentLevel = transform.GetComponentInParent<LevelAssetController>()) : _parentLevel;
		set => _parentLevel = value;
	}

	[SerializeField]
	private bool _removeOnStart = false;

	[SerializeField]
	private int _gridSubdivistions = 1;

	public bool autoSnap = true;
	public bool slerp = false;
	public float autoSnapTime = 0.1f;
	public bool offsetSnap = true;
	public Vector3 cellOffset => offsetSnap ? ParentLevel.Tilemap.layoutGrid.cellSize * 0.5f : Vector3.zero;
	public Grid grid => ParentLevel.Tilemap.layoutGrid;

	public bool IsSnapped => _snapCoroutine == null && GetSnapDistance() < maxCellDistance;

	private float _interpStart;
	private Vector3 _interpStartPos;
	private Coroutine _snapCoroutine;

	public Vector3 GetSnapPosition()
	{
		Grid grid = ParentLevel.Tilemap.layoutGrid;
		Vector3 r = grid.CellToWorld(grid.WorldToCell(transform.position));
		if (offsetSnap)
			r += grid.cellSize * 0.5f;
		return r;
	}

	public Vector3Int GetCellPosition()
	{
		return ParentLevel.Tilemap.layoutGrid.WorldToCell(transform.position);
	}

	/// <summary>
	/// get the distance that the object has to travel to be snapped to the grid
	/// </summary>
	public float GetSnapDistance()
	{
		// calculate distance from cell center to object position
		Grid grid = ParentLevel.Tilemap.layoutGrid;
		float dist = Vector3.Distance(GetSnapPosition(), transform.position);

		return dist;
	}

	/// <summary>
	/// returns an array of colliders that are in the target cell, which is the cell that is offset from this 
	/// object's cell. For example, an offset of Vector3(0, 1, 0) would return all the colliders of the objects
	/// in the cell directly below this object.
	/// </summary>
	/// <param name="offset">the cell offset from the cell that this object exists in</param>
	public Collider2D[] CollisionsAtOffset(Vector3Int offset)
	{
		Vector3Int targetCell = GetCellPosition() + offset;
		Vector2 targetPos = grid.CellToWorld(targetCell) + cellOffset;
		Collider2D[] cols = Physics2D.OverlapBoxAll(targetPos, new Vector2(0.9f, 0.9f), 0);

		return cols;
	}

	public void CheckForSnap()
	{
		if(_snapCoroutine != null)
			return;

		// calculate distance from cell center to object position
		float dist = GetSnapDistance();

		// if outside the snap threshold, snap it to grid
		if(dist > maxCellDistance)
		{
			SnapToGrid(autoSnapTime);
		}
	}

	public void SnapToGrid(float time = 0)
	{
		Grid grid = ParentLevel.Tilemap.layoutGrid;
		SnapToCell(grid.WorldToCell(transform.position), time);
	}

	public void SnapToCell(Vector3Int cell, float time)
	{
		// if the coroutine is running, stop it
		if(_snapCoroutine != null)
		{
			StopCoroutine(_snapCoroutine);
			_snapCoroutine = null;
		}

		_interpStart = Time.time;
		_interpStartPos = transform.position;
		Vector3 destination = ParentLevel.Tilemap.layoutGrid.CellToWorld(cell) + cellOffset;

		// define coroutine
		IEnumerator doSnap()
		{
			// while the interpolation time has not elapsed
			while (Time.time < _interpStart + time)
			{
				float delta = (Time.time - _interpStart) / time;
				Vector3 tpos = slerp ? 
					Vector3.Slerp(_interpStartPos, destination, delta) : 
					Vector3.Lerp(_interpStartPos, destination, delta);

				// apply the interpolated position and wait for next frame
				transform.position = tpos;
				yield return null;
			}

			// apply the final destination
			transform.position = destination;

			// stop the coroutine
			_snapCoroutine = null;
			yield break;
		}

		// start the coroutine
		_snapCoroutine = StartCoroutine(doSnap());
	}

	public void MoveCells(Vector3Int movement, float time = 0.2f)
	{
		Vector3Int currentCell = ParentLevel.Tilemap.layoutGrid.WorldToCell(transform.position);
		SnapToCell(currentCell + movement, time);
	}

	private void Start()
	{
		if (_removeOnStart)
			Destroy(this);
	}

	private void Update()
	{
		if (autoSnap)
			CheckForSnap();
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(GridSnap))]
	private class GridSnapEditor : Editor
	{
		protected GridSnap rTarget => target as GridSnap;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if(GUILayout.Button("Snap To Grid"))
			{
				if (rTarget._gridSubdivistions > 1)
					rTarget.transform.position -= rTarget.cellOffset / 2;
				rTarget.transform.position *= rTarget._gridSubdivistions;
				rTarget.transform.position = rTarget.GetSnapPosition();
				rTarget.transform.position /= rTarget._gridSubdivistions;
				if (rTarget._gridSubdivistions > 1)
					rTarget.transform.position += rTarget.cellOffset / 2;

				EditorUtility.SetDirty(rTarget);
			}
		}
	}
#endif
}

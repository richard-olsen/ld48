using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAssetController : MonoBehaviour
{

	[SerializeField]
	private Transform _levelBeginPosition;
	public Transform LevelBeginPosition => _levelBeginPosition;

	[SerializeField]
	private Transform _levelEndPosition;
	public Transform LevelEndPosition => _levelEndPosition;

	[SerializeField]
	private LevelAssetController _previousLevelPrefab;
	public LevelAssetController PreviousLevelPrefab => _previousLevelPrefab;
	private LevelAssetController _prevLevel;

	[SerializeField]
	private LevelAssetController _nextLevelPrefab;
	public LevelAssetController NextLevelPrefab => _nextLevelPrefab;
	private LevelAssetController _nextLevel;

	public void LoadPrevLevelAt(Vector3 position)
	{
		// instantiate the game object if it has not already been created
		if(_prevLevel == null)
		{
			_prevLevel = Instantiate(_previousLevelPrefab);
		}

		// enable and position the level game object
		_prevLevel.gameObject.SetActive(true);

		// calculate offset of level center from level beginning transform
		Vector3 offset = _prevLevel.LevelEndPosition.position - _prevLevel.transform.position;
		_prevLevel.transform.position = position - offset;

		// set the previous level's next level to this
		_prevLevel._nextLevel = this;
	}

	public void LoadNextLevelAt(Vector3 position)
	{
		// instantiate the game object if it has not already been created
		if (_nextLevel == null)
		{
			_nextLevel = Instantiate(_nextLevelPrefab);
		}

		// enable and position the level game object
		_nextLevel.gameObject.SetActive(true);

		// calculate offset of level center from level beginning transform
		Vector3 offset = _nextLevel.LevelBeginPosition.position - _nextLevel.transform.position;
		_nextLevel.transform.position = position - offset;

		// set the next level's previous level to this
		_nextLevel._prevLevel = this;
	}
}

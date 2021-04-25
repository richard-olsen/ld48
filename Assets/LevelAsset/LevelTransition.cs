using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class LevelTransition : MonoBehaviour
{
	[SerializeField]
	private LevelAssetController _parentLevel;

	[SerializeField]
	private Transform _camFocusPoint = null;

	[SerializeField]
	private LevelAssetController _targetLevelPrefab = null;
	private LevelAssetController _targetLevelInstance = null;

	[SerializeField, HideInInspector]
	private int _targetTransitionIndex = 0;

	// keeps track of whether user has already used this trigger so it doesn't get hit multiple times
	private bool _isTransitioning = false;

	protected LevelTransition targetTransition
	{
		get
		{
			LevelAssetController trgLev = _targetLevelInstance ?? _targetLevelPrefab;
			return trgLev.GetComponentsInChildren<LevelTransition>()[_targetTransitionIndex];
		}
	}

	private Camera _cam;
	protected Camera cam => _cam ?? (_cam = FindObjectOfType<Camera>());

	private Coroutine _camLerpCoroutine = null;

	private static readonly float cameraPanSpeed = 20;

	private void FocusCamera()
	{
		// only start the coroutine if it has not been started yet
		if (_camLerpCoroutine == null)
		{
			// define the camera lerp coroutine
			IEnumerator _doCamLerp()
			{
				// inch camera closer while it isn't close enough to the target
				while (
					Vector2.Distance(cam.transform.position, _camFocusPoint.position) >
					Time.deltaTime * cameraPanSpeed)
				{
					Vector2 camDelta = ((Vector2)_camFocusPoint.position - (Vector2)cam.transform.position).normalized * Time.deltaTime * cameraPanSpeed;
					cam.transform.position += new Vector3(camDelta.x, camDelta.y, 0);
					yield return null;
				}

				// snap the camera position to the target
				cam.transform.position = _camFocusPoint.position + Vector3.back * 10;

				// break the corouting and set it to null to flag that it's not happening any more
				_camLerpCoroutine = null;
				targetTransition._parentLevel.gameObject.SetActive(false);
				yield break;
			};

			// start the camera lerp async coroutine
			_camLerpCoroutine = StartCoroutine(_doCamLerp());
		}
	}

	public void DoTransition()
	{
		// do not hit the trigger more than once
		if (_isTransitioning)
			return;

		// get the target transition of the prefab
		LevelTransition target = targetTransition;
		LevelAssetController trgLev = _targetLevelInstance ?? _targetLevelPrefab;
		Vector3 offset = target.transform.position - trgLev.transform.position;

		// create the level instance if it has not been instantiated yet
		if (_targetLevelInstance == null)
		{
			_targetLevelInstance = Instantiate(_targetLevelPrefab);

			// get the target transition of the level INSTANCE, not the prefab
			target = targetTransition;
		}

		// set the level's position to match up with the transition point of this instance
		_targetLevelInstance.transform.position = transform.position - offset;
		_targetLevelInstance.gameObject.SetActive(true);
		target._targetLevelInstance = _parentLevel;

		// set flag so that transition only happens once
		target._isTransitioning = true;
		_isTransitioning = true;

		target.FocusCamera();
	}

	#region Unity Messages

#if UNITY_EDITOR
	private void OnEnable()
	{
		if (_parentLevel != null)
			return;

		LevelAssetController lvl = transform.parent.GetComponent<LevelAssetController>();
		if (lvl != null)
		{
			_parentLevel = lvl;
			EditorUtility.SetDirty(this);
		}
	}
#endif

	private void OnDisable()
	{
		if (_camLerpCoroutine != null)
		{
			StopCoroutine(_camLerpCoroutine);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if it's a player that's colliding with it
		if(collision.gameObject.GetComponent<Player>() != null)
			DoTransition();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_isTransitioning)
		{
			if (collision.gameObject.GetComponent<Player>() != null)
				_isTransitioning = false;
		}
	}

	#endregion

#if UNITY_EDITOR
	[CustomEditor(typeof(LevelTransition))]
	private class LevelTransitionEditor : Editor
	{
		protected LevelTransition rTarget => target as LevelTransition;

		int targetTransitionIndex
		{
			get
			{
				// not target level specified
				if (rTarget._targetLevelPrefab == null)
					return -1;

				LevelTransition[] transitions = rTarget._targetLevelPrefab.GetComponentsInChildren<LevelTransition>();

				// no transition points in target level
				if (transitions.Length <= 0)
					return -1;

				// find first transition point if it hasn't been set yet there is one
				if(rTarget._targetTransitionIndex < 0 || rTarget._targetTransitionIndex >= transitions.Length)
					rTarget._targetTransitionIndex = 0;

				return rTarget._targetTransitionIndex;
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (rTarget._targetLevelPrefab == null)
				return;

			GUILayout.Space(15);
			GUILayout.Label("Target transition destination:");

			LevelTransition[] allTrans = rTarget._targetLevelPrefab.GetComponentsInChildren<LevelTransition>();
			if (allTrans.Length <= 0)
				return;

			LevelTransition targTrans = allTrans[targetTransitionIndex];
			bool changeTarg = GUILayout.Button(targTrans.name);

			if (changeTarg)
			{
				rTarget._targetTransitionIndex++;
				_ = targetTransitionIndex;
				EditorUtility.SetDirty(rTarget);
			}
		}
	}
#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LvlTriggerType
{
	PreviousLevel,
	NextLevel,
	FocusCamera,
	GameOver
}

public class LevelTrigger : MonoBehaviour
{
	private LevelAssetController _parentLevel;
	public LevelAssetController Level => 
		_parentLevel ?? (_parentLevel = transform.GetComponentInParent<LevelAssetController>());

	private Transform _cameraFocusTarget => transform.GetChild(0) ?? transform;

	private Camera _cam;
	protected Camera cam => _cam ?? (_cam = FindObjectOfType<Camera>());

	private Coroutine _camLerpCoroutine = null;

	[SerializeField]
	private LvlTriggerType _triggerType;

	[SerializeField]
	private LayerMask _triggerLayers;

	private static float cameraPanSpeed = 5;

	private void FocusCamera()
	{
		// only start the coroutine if it has not been started yet
		if(_camLerpCoroutine == null)
		{
			// define the camera lerp coroutine
			IEnumerator _doCamLerp()
			{
				// inch camera closer while it isn't close enough to the target
				while (
					Vector2.Distance(cam.transform.position, _cameraFocusTarget.position) > 
					Time.deltaTime * cameraPanSpeed)
				{
					Vector3 camDelta = (_cameraFocusTarget.position - cam.transform.position) * Time.deltaTime * cameraPanSpeed;
					camDelta.z = 0;
					cam.transform.position += camDelta;
					yield return null;
				}

				// snap the camera position to the target
				cam.transform.position = _cameraFocusTarget.position + Vector3.back * 10;

				// break the corouting and set it to null to flag that it's not happening any more
				_camLerpCoroutine = null;
				yield break;
			};

			// start the camera lerp async coroutine
			_camLerpCoroutine = StartCoroutine(_doCamLerp());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		bool passesFilter = (_triggerLayers.value | (1 << collision.gameObject.layer)) > 0;
		if (!passesFilter)
			return;

		switch (_triggerType)
		{
			case LvlTriggerType.NextLevel:
				Level.LoadNextLevelAt(transform.position);
				break;
			case LvlTriggerType.PreviousLevel:
				Level.LoadPrevLevelAt(transform.position);
				break;
			case LvlTriggerType.FocusCamera:
				FocusCamera();
				break;
		}
	}

	private void OnDisable()
	{
		if(_camLerpCoroutine != null)
		{
			StopCoroutine(_camLerpCoroutine);
		}
	}
}

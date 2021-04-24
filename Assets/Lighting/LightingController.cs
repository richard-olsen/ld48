using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
	/// <summary>
	/// The lighting of the scene will change based on the position of this specified camera
	/// </summary>
	public Camera TargetCam => _targetCam;
	[
		SerializeField,
		Tooltip("The lighting of the scene will change dynamically based on the position of this camera")
	]
	private Camera _targetCam = null;

	/// <summary>
	/// handles dimming the global lighting of the scene based on the specified position
	/// </summary>
	/// <param name="targetPos">the target to emulate the lighting for</param>
	private void handleGlobalLightDimming(Vector3 targetPos)
	{

	}

	#region Unity Messages

	private void Start()
	{
		// if the target camera is not specified, throw an error
		if(_targetCam == null)
		{
			Debug.LogError("Target Camera must be specified");
			throw new System.NullReferenceException("_targetCam is null");
		}
	}

	private void Update()
	{
		handleGlobalLightDimming(TargetCam.transform.position);
	}

	#endregion
}

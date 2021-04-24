using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

#if UNITY_EDITOR
// access unity editor scripting capabilities
using UnityEditor;
#endif

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
	/// The global light that will be modified to simulate the deeper darker light
	/// </summary>
	public Light2D GlobalLight => _globalLight;
	[
		SerializeField,
		Tooltip("The global light object (should be attached to the lit camera)")
	]
	private Light2D _globalLight = null;

	[SerializeField, Tooltip("The depth at which the brightest light level will be dimmed to")]
	private float _lowDepth = -100;
	[SerializeField, Tooltip("The depth at which the dimmest light level will be dimmed to")]
	private float _highDepth = 0;

	[SerializeField, Tooltip("The lighting color at the high depth")]
	private Color _brightColor = new Color(1, 1, 1);
	[SerializeField, Tooltip("The lighting color at the low depth")]
	private Color _dimColor = new Color(0, 0, 0);

	[SerializeField, Tooltip("The color of the camera background when it's at the surface")]
	private Color _camBGBright = new Color(100, 100, 100);
	[SerializeField, Tooltip("The color of the camera background when it's at maximum sea depth")]
	private Color _camBGDim = new Color(0, 0, 0);

	/// <summary>
	/// handles dimming the global lighting of the scene based on the specified position
	/// </summary>
	/// <param name="targetPos">the target to emulate the lighting for</param>
	private void handleGlobalLightDimming(Vector3 targetPos)
	{
		// calculate the lerp delta value
		float delta = targetPos.y;
		if (delta < _lowDepth)
			delta = _lowDepth;
		if (delta > _highDepth)
			delta = _highDepth;
		delta /= -Mathf.Abs(_lowDepth - _highDepth);

		// get the appropriate color by interpolating between the two min and max depth lighting colors 
		// and apply it to the global light
		Color targetLightColor = Color.Lerp(_brightColor, _dimColor, delta);
		GlobalLight.color = targetLightColor;

		// change the light intensity based on how deep the camera is
		GlobalLight.intensity = 1 - delta;

		// interpolate between the two specified camera background colors for bright and dark
		Color targetCamColor = Color.Lerp(_camBGBright, _camBGDim, delta);
		TargetCam.backgroundColor = targetCamColor;
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

	#region Editor Scripting
#if UNITY_EDITOR

	[CustomEditor(typeof(LightingController))]
	public class LightningControllerEditor : Editor
	{
		// create a specific typed property of the target
		public LightingController lightingController => target as LightingController;

		// override inspector GUI
		public override void OnInspectorGUI()
		{

			// draw default unity controls
			base.OnInspectorGUI();

			// draw custom controls
			handleInspectorGUI();
		}

		// handle custom GUI control
		private void handleInspectorGUI()
		{

			// don't render custom controls if unity is in play mode
			if (Application.isPlaying)
				return;

			// if the user presses a button labelled "Update Lighting"
			if (GUILayout.Button("Update Lighting"))
			{
				// update the global lighting thing
				lightingController.handleGlobalLightDimming(lightingController.TargetCam.transform.position);
			}
		}
	}

#endif
	#endregion
}
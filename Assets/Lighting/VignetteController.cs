using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[SelectionBase]
public class VignetteController : MonoBehaviour
{
	[SerializeField]
	private Color _vignetteColor;

	private void applyColor()
	{
		foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
		{
			sr.color = _vignetteColor;
		}
	}
	
#if UNITY_EDITOR
	[CustomEditor(typeof(VignetteController))]
	private class VignetteControllerEditor : Editor
	{
		public VignetteController rTarget => target as VignetteController;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if(GUILayout.Button("Apply Color"))
			{
				rTarget.applyColor();
			}
		}
	}
#endif
}


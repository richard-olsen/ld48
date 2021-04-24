using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
// access unity editor scripting
using UnityEditor;
#endif

public class HUDController : MonoBehaviour
{
	/// <summary>
	/// A reference to the player that this HUD is displaying the information of
	/// </summary>
	public Player Player => _player;
	[
		SerializeField,
		Tooltip("The player who's stats are reflected by this HUD")
	]
	private Player _player;

	[SerializeField]
	private OxyMeterController _oxyMeterController = null;
	[SerializeField]
	private ActionContainerController _actionButtonContainer = null;

	#region Unity Messages

	private void Start() { }

	private void Update(){ 
	}

	#endregion

	#region Editor Scripting
#if UNITY_EDITOR

	[CustomEditor(typeof(HUDController))]
	public class HUDControllerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}

#endif
	#endregion
}

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
	private Slider _oxygenMeterSlider;

	private void updateOxygenMeterLevel()
	{
		// get the oxygen level between 0 and 1
		float oxDelta = (float)Player.GetOxygenLevel() / Player.GetMaxOxygenLevel();

		// set the oxygen meter slider to the player's oxygen level
		_oxygenMeterSlider.value = oxDelta;
	}

	#region Unity Messages

	private void Update()
	{
		updateOxygenMeterLevel();
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

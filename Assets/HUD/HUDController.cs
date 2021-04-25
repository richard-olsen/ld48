using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
// access unity editor scripting
using UnityEditor;
#endif

[RequireComponent(typeof(Canvas))]
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
	private EventSystem _eventSys;
	public EventSystem EventSys => _eventSys;

	private Camera _camera;
	public Camera Camera => _camera ?? (_camera = Camera.main);

	private Canvas _canvas;
	public Canvas CanvasComponent => _canvas ?? (_canvas = GetComponent<Canvas>());

	[SerializeField]
	private OxyMeterController _oxyMeterController = null;
	[SerializeField]
	private ActionContainerController _actionButtonContainer = null;
	public ActionContainerController ActionButtonContainer => _actionButtonContainer;
	[SerializeField]
	private WorldToScreenCursor _worldScreenCursor;
	public WorldToScreenCursor WorldScreenCursor => _worldScreenCursor;

	#region Unity Messages

	private void Start() {
		Player.GetComponent<PlayerInteraction>().HUD = this;
	}

	private void Update(){ }

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

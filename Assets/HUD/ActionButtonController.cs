using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ActionButtonController : MonoBehaviour
{
	[SerializeField, HideInInspector]
	private HUDController _hudParent;
	public HUDController HUDParent { get => _hudParent; set => _hudParent = value; }

	[SerializeField]
	private PlayerAction _playerAction;

	[SerializeField]
	private Button _button;
	public Button ButtonComponent => _button;

	[SerializeField]
	private TextMeshProUGUI _text;
	public TextMeshProUGUI TextComponent => _text;

	/// <summary>
	/// set the player action to the specified instance of the scriptable object
	/// </summary>
	public void SetPlayerAction(PlayerAction action)
	{
		_playerAction = action;
	}

	/// <summary>
	/// get or set the text displayed on the action button
	/// </summary>
	public string Text
	{
		get => _text.text;
		set
		{
			_text.text = value;
		}
	}

	/// <summary>
	/// refreshes the info action data based on the playerAction scripted object
	/// </summary>
	public void RefreshActionData()
	{
		if (_playerAction == null)
			return;
		Text = _playerAction.ActionTitle;
	}

	private void hookClickEvent()
	{
		// define button click action
		void onclick() {
			PlayerInteraction plrInt = _hudParent.Player.GetComponent<PlayerInteraction>();
			plrInt.SelectPlayerAction(_playerAction);
		};

		// attach the event listener to the button's on click event
		Button button = GetComponent<Button>();
		button.onClick.AddListener(new UnityEngine.Events.UnityAction(onclick));
	}

	#region Unity Messages

	private void OnEnable()
	{
		RefreshActionData();
		hookClickEvent();
	}

	#endregion
}

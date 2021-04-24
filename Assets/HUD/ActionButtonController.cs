using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonController : MonoBehaviour
{
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
		Text = _playerAction.ActionTitle;
	}

	#region Unity Messages

	private void OnEnable()
	{
		RefreshActionData();
	}

	#endregion
}

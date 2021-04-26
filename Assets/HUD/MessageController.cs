using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _messageText;
	[SerializeField]
	private Image _spacebarImage;

	[SerializeField]
	private Sprite _spacebarUp;
	[SerializeField]
	public Sprite _spacebarPressed;

	private Player _plr;
	protected Player plr => _plr ?? (_plr = FindObjectOfType<Player>());

	private bool _buttonWasPressed = false;
	public bool IsShowing => gameObject.activeInHierarchy;

	public void ShowMessage(string message)
	{
		Time.timeScale = 0;
		plr.usingMenus = true;

		_spacebarImage.sprite = _spacebarUp;
		_buttonWasPressed = false;

		gameObject.SetActive(true);
		_messageText.text = message;
	}

	public void Hide()
	{
		Time.timeScale = 1;
		plr.usingMenus = false;
		gameObject.SetActive(false);
	}

	private void Update()
	{
		bool buttonPressed = Input.GetButton("Interact");
		if (buttonPressed)
		{
			_buttonWasPressed = true;
			_spacebarImage.sprite = _spacebarPressed;
		}
		else
		{
			if (_buttonWasPressed)
			{
				Hide();
				HUDController.Noise_SubmitValid();
			}
		}
	}
}

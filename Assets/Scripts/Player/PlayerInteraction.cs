using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerInteractMode
{
	OpenMenu,
	SelectAction,
	SelectTile,
	ResumeGame
}

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
	private Player _playerComponent;
	public Player PlayerComponent => _playerComponent ?? (_playerComponent = GetComponent<Player>());

	private HUDController _hud;
	public HUDController HUD { get => _hud; set => _hud = value; }

	private Vector2Int _lastAxis = Vector2Int.zero;

	protected ActionContainerController actionButtons => HUD.ActionButtonContainer;

	private PlayerInteractMode _interactMode = PlayerInteractMode.OpenMenu;

	public void InteractWithWorld()
	{
		_interactMode = PlayerInteractMode.SelectTile;
	}

	public void SelectPlayerAction(PlayerAction action)
	{
		switch (action.ActionType)
		{
			// TODO
		}
	}

	private void openMenu()
	{
		actionButtons.Open();
		_interactMode = PlayerInteractMode.SelectAction;
		HUD.EventSys.SetSelectedGameObject(actionButtons.GetAllButtons()[0].gameObject);
	}

	private void handleInput()
	{
		bool interactPressed = Input.GetButtonDown("Interact");
		if (interactPressed)
		{
			switch (_interactMode)
			{
				case PlayerInteractMode.OpenMenu:
					openMenu();
					break;
				case PlayerInteractMode.SelectAction:
					// do nothing because the button has an event
					break;
			}
		}

		float xIn = Input.GetAxis("Horizontal");
		float yIn = Input.GetAxis("Vertical");
		Vector2Int move = Vector2Int.zero;
		if (Mathf.Abs(xIn) > 0.5)
			move.x += (int)Mathf.Sign(xIn);
		if (Mathf.Abs(yIn) > 0.5)
			move.y += (int)Mathf.Sign(yIn);

		if(_lastAxis.x != move.x)
		{
			// move right
			if (move.x > 0) { }

			// move left
			else if (move.x < 0) { }
		}
		if(_lastAxis.y != move.y)
		{
			// move up
			if (move.y > 0) { }

			// move down
			else if (move.y < 0) { }
		}

		// store last axis
		_lastAxis = move;
	}

	#region Unity Messages

	private void Update()
	{
		handleInput();
	}

	#endregion
}

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
	private LevelAssetController _currentLevel;
	public LevelAssetController CurrentLevel
	{
		get
		{
			if (_currentLevel == null || !_currentLevel.gameObject.activeInHierarchy)
				_currentLevel = FindObjectOfType<LevelAssetController>();

			return _currentLevel;
		}
		set
		{
			_currentLevel = value;
		}
	}

	private Player _playerComponent;
	public Player PlayerComponent => _playerComponent ?? (_playerComponent = GetComponent<Player>());

	private PlayerAction _currentAction;
	public PlayerAction CurrentAction => _currentAction;

	private HUDController _hud;
	public HUDController HUD { get => _hud; set => _hud = value; }

	private Vector2Int _lastAxis = Vector2Int.zero;

	protected ActionContainerController actionButtons => HUD.ActionButtonContainer;

	private PlayerInteractMode _interactMode = PlayerInteractMode.OpenMenu;
	private GridSnap _worldCursor = null;
	public GridSnap WorldCursor => _worldCursor;

	public void InteractWithWorld()
	{
		_interactMode = PlayerInteractMode.SelectTile;
		PlayerComponent.usingMenus = true;
		WorldCursor.transform.parent = CurrentLevel.transform;
		WorldCursor.gameObject.SetActive(true);
		WorldCursor.transform.position = transform.position;
		WorldCursor.SnapToGrid();
		HUD.WorldScreenCursor.Show();
	}

	public void SelectPlayerAction(PlayerAction action)
	{
		_currentAction = action;
		switch (action.ActionType)
		{
			case PlayerActionType.Interact:
				InteractWithWorld();
				actionButtons.Close();
				break;
		}
	}

	private void openMenu()
	{
		actionButtons.Open();
		_interactMode = PlayerInteractMode.SelectAction;
		HUD.EventSys.SetSelectedGameObject(actionButtons.GetAllButtons()[0].gameObject);
		PlayerComponent.usingMenus = true;
	}

	private void handleCursorMovement(Vector2Int curMove)
	{
		// only handle cursor movement if in selectTile mode
		if (_interactMode != PlayerInteractMode.SelectTile)
			return;

		if (WorldCursor.IsSnapped)
		{
			if(curMove.magnitude > 0)
				WorldCursor.MoveCells((Vector3Int)curMove);
		}
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

		Vector2Int moveDown = Vector2Int.zero;

		if(_lastAxis.x != move.x)
		{
			// move right
			if (move.x > 0)
				moveDown.x++;

			// move left
			else if (move.x < 0)
				moveDown.x--;
		}
		if(_lastAxis.y != move.y)
		{
			// move up
			if (move.y > 0)
				moveDown.y++;

			// move down
			else if (move.y < 0)
				moveDown.y--;
		}

		handleCursorMovement(moveDown);

		// store last axis
		_lastAxis = move;
	}

	#region Unity Messages

	private void OnEnable()
	{
		
		if(_worldCursor == null)
		{
			_worldCursor = (new GameObject()).AddComponent<GridSnap>();
			_worldCursor.name = "PlayerWorldCursor";
			_worldCursor.slerp = true;
			_worldCursor.autoSnap = false;
			_worldCursor.gameObject.SetActive(false);
		}
		_worldCursor.transform.parent = CurrentLevel.transform;
	}

	private void Update()
	{
		handleInput();
	}

	#endregion
}

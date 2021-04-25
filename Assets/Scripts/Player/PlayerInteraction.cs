using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerInteractMode
{
	OpenMenu,
	SelectAction,
	SelectWorld,
	Interactible,
	ResumeGame
}

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour, IInteractor
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

	private IInteractible _currentInteractible;
	private PlayerAction _currentAction;
	public PlayerAction CurrentAction => _currentAction;

	private HUDController _hud;
	public HUDController HUD { get => _hud; set => _hud = value; }

	private Vector2Int _lastAxis = Vector2Int.zero;

	protected ActionContainerController actionButtons => HUD.ActionButtonContainer;

	private PlayerInteractMode _interactMode = PlayerInteractMode.OpenMenu;
	private GridSnap _worldCursor = null;
	public GridSnap WorldCursor => _worldCursor;

	public void EndInteractCycle()
	{
		_interactMode = PlayerInteractMode.OpenMenu;
		PlayerComponent.usingMenus = false;
		WorldCursor.gameObject.SetActive(false);
		_currentAction = null;
		_currentInteractible = null;
	}

	public void StartSelectWorld()
	{
		_interactMode = PlayerInteractMode.SelectWorld;
		PlayerComponent.usingMenus = true;
		WorldCursor.transform.parent = CurrentLevel.transform;
		WorldCursor.gameObject.SetActive(true);
		WorldCursor.transform.position = transform.position;
		WorldCursor.SnapToGrid();
		HUD.WorldScreenCursor.Show(0);
	}

	public void SelectInteractible(IInteractible interactible)
	{
		_currentInteractible = interactible;
		_interactMode = PlayerInteractMode.Interactible;
		HUD.WorldScreenCursor.Show(1);
	}

	public void SelectPlayerAction(PlayerAction action)
	{
		// this needs to be done in the next frame otherwise the interaction input "double clicks"
		// and sometimes causes the player to not be able to do any action

		// do inside a coroutine
		IEnumerator doNextFrame(){

			// wait for some time
			yield return new WaitForSecondsRealtime(0.1f);

			_currentAction = action;
			switch (action.ActionType)
			{
				case PlayerActionType.Interact:
					StartSelectWorld();
					actionButtons.Close();
					break;
			}

			// stop coroutine
			yield break;
		}

		// start the coroutine so that this action does not occur until next frame
		StartCoroutine(doNextFrame());
	}

	private void selectWorld(Vector3Int cellPos)
	{
		if (CurrentAction.ActionType == PlayerActionType.Interact)
		{
			// find any interactible objects at the cell that the player selected
			Vector3 selectPos = CurrentLevel.Tilemap.layoutGrid.CellToWorld(cellPos) + CurrentLevel.Tilemap.layoutGrid.cellSize * 0.5f;
			Collider2D[] colliders = Physics2D.OverlapBoxAll(selectPos, new Vector2(0.9f, 0.9f), 0);
			bool interacted = false;
			for (int i = colliders.Length - 1; i >= 0; i--)
			{
				Collider2D collider = colliders[i];
				IInteractible interactible = collider.GetComponent<IInteractible>();
				if (interactible != null)
				{
					// interact with the interactible object that was found
					interactible.InteractWith(this);
					interacted = true;
					break;
				}
			}

			if (!interacted)
				EndInteractCycle();
		}
	}

	private void openMenu()
	{
		actionButtons.Open();
		_interactMode = PlayerInteractMode.SelectAction;
		HUD.EventSys.SetSelectedGameObject(actionButtons.GetAllButtons()[0].gameObject);
		PlayerComponent.usingMenus = true;
	}

	private void interact()
	{

		switch (_interactMode)
		{
			case PlayerInteractMode.OpenMenu:
				openMenu();
				break;
			case PlayerInteractMode.SelectAction:
				// do nothing because the button has an event
				break;
			case PlayerInteractMode.SelectWorld:
				selectWorld(WorldCursor.GetCellPosition());
				break;
		}
	}

	private void handleInteractMovement(Vector2Int curMove)
	{
		if (curMove.magnitude <= 0)
			return;

		// only handle cursor movement if in selectTile mode
		if (_interactMode == PlayerInteractMode.SelectWorld)
		{
			if (WorldCursor.IsSnapped)
			{
				Vector3Int targetCell = WorldCursor.GetCellPosition() + (Vector3Int)curMove;
				Vector3Int plrCell = WorldCursor.grid.WorldToCell(transform.position);

				// restrict movement to tiles only 1 cell away or closer
				if (Mathf.Abs(targetCell.x - plrCell.x) > 1 || Mathf.Abs(targetCell.y - plrCell.y) > 1)
					return;

				WorldCursor.SnapToCell(targetCell, 0.15f);
			}
		}

		// interact with physical objects
		if (_interactMode == PlayerInteractMode.Interactible)
		{
			GridSnap gs = _currentInteractible.gameObject.GetComponent<GridSnap>();
			Vector3Int targetCell = gs.GetCellPosition() + (Vector3Int)curMove;
			Vector3 targetPos = gs.grid.CellToWorld(targetCell) + gs.cellOffset;

			// check to see if player can move to taget position
			Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPos, new Vector2(0.9f, 0.9f), 0);
			bool canMove = true;
			bool playerInWay = false;
			if (colliders.Length > 0)
			{
				foreach(Collider2D col in colliders)
				{
					if(col.gameObject.GetComponent<Player>() != null)
					{
						playerInWay = true;
					}
					else
					{
						canMove = false;
						break;
					}
				}
			}

			// called when interaction was successful
			void finishInteract()
			{

				IEnumerator finishAfterWait()
				{
					gs.SnapToCell(targetCell, 0.4f);
					yield return new WaitForSeconds(0.25f);
					EndInteractCycle();
					yield break;
				}

				StartCoroutine(finishAfterWait());
			}

			// if the interactible can be moved to the target position
			if (canMove)
			{
				if (playerInWay)
				{
					if (PlayerComponent.MoveAlongGrid(curMove.x, curMove.y))
						finishInteract();
				}
				else finishInteract();
			}
		}
	}

	private void handleInput()
	{
		bool interactPressed = Input.GetButtonDown("Interact");
		if (interactPressed)
		{
			interact();
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

		handleInteractMovement(moveDown);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnInfoController : MonoBehaviour
{
	[SerializeField]
	private HUDController _hud;

	[SerializeField]
	private TextMeshProUGUI _turnStateText;

	[SerializeField]
	private TextMeshProUGUI _turnCountText;

	private Color _oCol;
	private string playerTurnText = "Your Turn";
	private string enemyTurnText = "Enemy's Turn";

	private TurnBasedMovementSystem _turnSystem;
	public TurnBasedMovementSystem TurnSystem => _turnSystem ?? (_turnSystem = FindObjectOfType<TurnBasedMovementSystem>());

	private void OnEnable()
	{
		_oCol = _turnStateText.color;
		_turnSystem = FindObjectOfType<TurnBasedMovementSystem>();
	}

	private void Update()
	{
		_turnCountText.text = Mathf.Max(TurnSystem.PlayerActionsLeft, 0).ToString();

		// player turn
		if (TurnSystem.IsPlayersTurn)
		{
			_turnStateText.color = _oCol;
			_turnStateText.text = playerTurnText;
		}

		// enemy turn
		else
		{
			_turnStateText.color = Color.gray;
			_turnStateText.text = enemyTurnText;
		}
	}
}

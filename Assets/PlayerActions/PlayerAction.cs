using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAction", menuName = "LD48/Player Action")]
public class PlayerAction : ScriptableObject
{
	[SerializeField]
	private string _actionTitle = "Default";
	public string ActionTitle => _actionTitle;

	[SerializeField]
	private float _oxygenCost = 1.0f;
	public float OxygenCost => _oxygenCost;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class OxyMeterController : MonoBehaviour
{
	[SerializeField]
	private HUDController _hud;
	public Player Player => _hud.Player;

	[SerializeField, Space]
	private Slider _oxyMeterSlider = null;
	[SerializeField]
	private Image _oxyMeterFG = null;
	[SerializeField]
	private Image _oxyMeterBG = null;

	[SerializeField, Space, Tooltip("The level at which oxygen is considered to be low")]
	private float _lowOxyLevel = 0.25f;
	[SerializeField]
	private Color _unsafeOxyColorFG = new Color(1, 0, 0);
	private Color _safeOxyColorFG;
	[SerializeField]
	private Color _unsafeOxyColorBg = new Color(0.5f, 0, 0);
	private Color _safeOxyColorBg;

	private void updateOxygenMeterLevel()
	{
		// get the oxygen level between 0 and 1
		float oxDelta = (float)Player.GetOxygenLevel() / Player.GetMaxOxygenLevel();

		// handles flashing oxymeter if low oxygen
		handleOxyMeterDynamicStyle(oxDelta);

		// set the oxygen meter slider to the player's oxygen level
		_oxyMeterSlider.value = oxDelta;
	}

	private void handleOxyMeterDynamicStyle(float oxDelta)
	{

		// if oxygen level low
		if (oxDelta < _lowOxyLevel)
		{
			_oxyMeterFG.color = _unsafeOxyColorFG;
			_oxyMeterBG.color = Time.unscaledTime % 0.5 < 0.25 ? _unsafeOxyColorBg : _safeOxyColorBg;
		}

		// if oxygen level is not low
		else
		{
			_oxyMeterFG.color = _safeOxyColorFG;
			_oxyMeterBG.color = Color.grey;
		}
	}

	#region Unity Messages

	private void Start()
	{
		// set the safe oxygen color to it's initial value when program starts
		_safeOxyColorBg = _oxyMeterBG.color;
		_safeOxyColorFG = _oxyMeterFG.color;
	}

	private void Update()
	{
		updateOxygenMeterLevel();
	}

	#endregion
}

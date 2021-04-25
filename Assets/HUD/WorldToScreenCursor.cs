using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreenCursor : MonoBehaviour
{
	private Vector2 oSizeDelta;

	[SerializeField]
	private HUDController _hud;
	private PlayerInteraction playerInteraction => _hud.Player.GetComponent<PlayerInteraction>();

	private RectTransform rectTr => transform as RectTransform;

	public void Show()
	{
		gameObject.SetActive(true);
		followWorldCursor();
	}

	private	void followWorldCursor()
	{
		GridSnap worldCursor = playerInteraction.WorldCursor;
		Vector3 screenpos = _hud.Camera.WorldToScreenPoint(worldCursor.transform.position);

		rectTr.anchorMin = Vector2.zero;
		rectTr.anchorMax = Vector2.zero;
		rectTr.anchoredPosition = screenpos;

		rectTr.sizeDelta = oSizeDelta * (Mathf.Cos(Time.time * Mathf.PI * 2) * 0.1f + 1);
	}

	private void OnEnable()
	{
		oSizeDelta = rectTr.sizeDelta;
	}

	private void OnDisable()
	{
		rectTr.sizeDelta = oSizeDelta;
	}

	private void Update()
	{
		// disable if player world cursor is disabled
		if (playerInteraction.WorldCursor == null || !playerInteraction.WorldCursor.gameObject.activeInHierarchy)
		{
			this.gameObject.SetActive(false);
		}
		else
		{
			followWorldCursor();
		}
	}
}

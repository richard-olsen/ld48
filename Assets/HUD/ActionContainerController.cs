using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionContainerController : MonoBehaviour
{
	[SerializeField]
	private HUDController _hud;
	public Player Player => _hud.Player;

	[SerializeField, Tooltip("The prefab that will be used to instantiate action buttons at runtime")]
	private ActionButtonController _actionButtonPrefab;

	/// <summary>
	/// Returns an array of all the action buttons contained in this container
	/// </summary>
	/// <returns></returns>
	public ActionButtonController[] GetAllButtons()
	{
		ActionButtonController[] r = transform.GetComponentsInChildren<ActionButtonController>();
		return r;
	}

	/// <summary>
	/// adds an action button to the action button container
	/// </summary>
	/// <param name="action">the action that the action button should be based off</param>
	public ActionButtonController AddAction(PlayerAction action)
	{

		// instantiate the action button prefab and apply the proper parameters
		ActionButtonController abCont = Instantiate(_actionButtonPrefab);
		abCont.SetPlayerAction(action);
		abCont.transform.parent = transform;

		// return the instantiated button
		return abCont;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}

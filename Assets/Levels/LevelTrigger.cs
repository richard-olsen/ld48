using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LvlTriggerType
{
	PreviousLevel,
	NextLevel,
	GameOver
}

public class LevelTrigger : MonoBehaviour
{
	private LevelAssetController _parentLevel;
	public LevelAssetController Level => 
		_parentLevel ?? (_parentLevel = transform.GetComponentInParent<LevelAssetController>());

	[SerializeField]
	private LvlTriggerType _triggerType;

	[SerializeField]
	private LayerMask _triggerLayers;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		bool passesFilter = (_triggerLayers.value | (1 << collision.gameObject.layer)) > 0;
		if (!passesFilter)
			return;

		switch (_triggerType)
		{
			case LvlTriggerType.NextLevel:
				Level.LoadNextLevelAt(transform.position);
				break;
			case LvlTriggerType.PreviousLevel:
				Level.LoadPrevLevelAt(transform.position);
				break;
		}
	}
}

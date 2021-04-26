using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
	[SerializeField]
	private LayerMask _layers;

	[SerializeField, TextArea]
	private string _message = "Lorem ipsum dolor semet";

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// if game object is included in layer mask
		if((collision.gameObject.layer | _layers.value) > 0)
		{
			HUDController.ShowMessage(_message);
		}
	}
}

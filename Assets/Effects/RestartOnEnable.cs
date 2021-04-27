using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		ParticleSystem ps = GetComponent<ParticleSystem>();
		ps.time = 0;
		ps.Play();
	}
}

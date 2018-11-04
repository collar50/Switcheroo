using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_AIRandom : MonoBehaviour, IMovementInput
{
	private float horizontal;
	private float vertical;
	[SerializeField] private float repeatRate;

	private void Start ()
	{
		InvokeRepeating ("RandomizeMovement", 0f, repeatRate);
	}

	private void Update ()
	{
		horizontal *= .5f;
		vertical *= .5f;
	}

	private void RandomizeMovement ()
	{		
		horizontal = Random.Range (0f, 10f);
		vertical = Random.Range (0f, 10f);
	}

	public float Horizontal ()
	{
		return horizontal;
	}

	public float Vertical ()
	{
		return vertical;
	}
}

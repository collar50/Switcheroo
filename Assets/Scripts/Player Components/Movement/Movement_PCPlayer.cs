using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_PCPlayer : MonoBehaviour, IMovementInput
{

	public float Horizontal ()
	{
		return Input.GetAxis ("Horizontal");
	}

	public float Vertical ()
	{
		return Input.GetAxis ("Vertical");
	}
}

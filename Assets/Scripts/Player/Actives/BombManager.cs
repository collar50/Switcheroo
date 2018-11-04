using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{

	public int damage;
	public int shieldStripper;
	public int fieldStripper;

	public void OnCollisionEnter2D ()
	{
		Destroy (this.gameObject);
	}
}

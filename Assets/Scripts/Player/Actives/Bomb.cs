using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BaseActive
{
	[SerializeField] private float startRadius;
	[SerializeField] private Transform bomb;
	[SerializeField] private int numBombs;
	[SerializeField] private int bombForce;

	private enum BombType
	{
		Base,
		ShieldRipper,
		FieldRipper,
	}
	// Use this for initialization
	protected override void StartActive ()
	{
		for (int i = 0; i < numBombs; i++) {
			float x = Mathf.Cos (2 * Mathf.PI * i / numBombs) * startRadius;
			float y = Mathf.Sin (2 * Mathf.PI * i / numBombs) * startRadius;
			Transform thisBomb = Instantiate (bomb, this.transform.position + new Vector3 (x, y, 0f), Quaternion.identity);
			thisBomb.tag = "Bomb" + this.gameObject.tag;
			thisBomb.GetComponent<Rigidbody2D> ().AddForce ((thisBomb.transform.position - this.transform.position) * bombForce);
		}
	}


	
	// Update is called once per frame
	protected override void EndActive ()
	{
		// Do nothing; bomb has no duration, it just happens
	}
}

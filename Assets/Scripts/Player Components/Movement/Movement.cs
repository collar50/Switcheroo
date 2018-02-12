using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

	private IMovementInput movementInput;
	private Rigidbody2D rb;
	[SerializeField] private float thrust;
	[SerializeField] private float velocityLimit;
	[SerializeField] private float friction;

	// Use this for initialization
	void Start ()
	{
		rb = this.GetComponent<Rigidbody2D> ();
		movementInput = this.GetComponent<IMovementInput> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Move ();
	}

	void Move ()
	{
		float horizontal = movementInput.Horizontal ();
		float vertical = movementInput.Vertical ();

		rb.velocity -= rb.velocity * friction * Time.deltaTime;

		if (rb.velocity.magnitude >= velocityLimit) {
			rb.velocity = rb.velocity.normalized * velocityLimit;
		}

		Vector2 force = new Vector2 (horizontal, vertical) * Time.deltaTime * thrust;
		rb.AddForce (force);
	}
}

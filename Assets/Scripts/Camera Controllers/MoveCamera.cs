/* Allows the camera to move around while creating a 
 * new level
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

	[SerializeField] float moveSpeed;
	[SerializeField] float zoomSpeed;
	private Camera cam;
	[SerializeField]private float horizontalLock;
	[SerializeField]private float verticalLock;

	void Start ()
	{
		cam = GetComponent<Camera> ();
	}

	void LateUpdate ()
	{
		Move ();
		Zoom ();
	}
	
	/* Use horizontal and vertical input to move the 
	 * camera
	 */
	void Move ()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		if ((vertical < 0 && transform.position.y > -verticalLock) ||
		    (vertical > 0 && transform.position.y < verticalLock)) {
			transform.Translate (new Vector2 (0, vertical) * moveSpeed * Time.deltaTime);
		}

		if ((horizontal < 0 && transform.position.x > -horizontalLock) ||
		    (horizontal > 0 && transform.position.x < horizontalLock)) {
			transform.Translate (new Vector2 (horizontal, 0) * moveSpeed * Time.deltaTime);
		}
	}

	void Zoom ()
	{
		float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");

		if ((cam.orthographicSize > 20f && mouseScroll > 0) ||
		    (cam.orthographicSize < 300f && mouseScroll < 0)) {
			cam.orthographicSize -= mouseScroll * zoomSpeed * Time.deltaTime;
		}
	}
}

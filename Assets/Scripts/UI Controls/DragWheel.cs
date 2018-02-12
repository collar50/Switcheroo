using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWheel : MonoBehaviour, IPointerDownHandler
{
	private bool mouseDown;
	private Vector2 previousMousePos;
	private RectTransform wheel;
	private Vector3[] wheelCorners = new Vector3[4];
	private float currentRotationSpeed;
	private float addedRotationSpeed;
	[SerializeField] private float torque;
	[SerializeField] private float rotationSpeedLimit;
	[SerializeField] private float rotationFriction;

	// Use this for initialization
	void Start ()
	{
		wheel = this.gameObject.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update ()
	{
		InitializeMouseLetUp ();

		addedRotationSpeed = 0f;

		if (mouseDown == true) {
			AddTorque ();
		}

		currentRotationSpeed = currentRotationSpeed * (1 - rotationFriction) + addedRotationSpeed;
		if (Mathf.Abs (currentRotationSpeed) > rotationSpeedLimit) {
			currentRotationSpeed = currentRotationSpeed / Mathf.Abs (currentRotationSpeed) * rotationSpeedLimit;
		}
		wheel.Rotate (new Vector3 (0f, 0f, currentRotationSpeed));
	}

	private void InitializeMouseHeldDown ()
	{
		if (Input.GetMouseButtonDown (0)) {
			mouseDown = true;
			previousMousePos = Input.mousePosition;
		}
	}

	private void InitializeMouseLetUp ()
	{
		if (Input.GetMouseButtonUp (0)) {
			mouseDown = false;
		}
	}

	private void AddTorque ()
	{
		if (mouseDown) {			
			// Get corner points of wheel UI object in world space
			wheel.GetWorldCorners (wheelCorners);

			// Calculate the center point of the wheel UI object in world space
			// I don't know the order in which GetWorldCorners stores the corners...
			float wheelCenterX = 0;
			float wheelCenterY = 0;

			for (int i = 0; i < wheelCorners.Length; i++) {
				wheelCenterX += .25f * (float)wheelCorners [i].x;
				wheelCenterY += .25f * (float)wheelCorners [i].y;
			}

			// Calculate the cross product of 2 vectors (which need to be expressed here as Vector3s:
			// 1) The vector between wheel centerpoint and where mouse is this frame
			// 2) The vector between wheel centerpoint and where mouse was last frame
			// This will tell us whether (from last frame to this frame) we've moved the mouse
			// clockwise or counterclockwise.
			float direction = Vector3.Cross (new Vector3 (previousMousePos.x - wheelCenterX, previousMousePos.y - wheelCenterY, 0f), 
				                  new Vector3 (Input.mousePosition.x - wheelCenterX, Input.mousePosition.y - wheelCenterY, 0f)).normalized.z;
			
			// As long as we've moved the mouse from one frame to the next, rotate the wheel
			// according to which direction we rotated. 
			if (direction != 0) {
				addedRotationSpeed = direction * torque * Time.deltaTime;
			}
			// Store this frame's mouse position, to be used next frame (unless mouse is let up)
			previousMousePos = Input.mousePosition;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		InitializeMouseHeldDown ();
	}
}

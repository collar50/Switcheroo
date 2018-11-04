using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private bool mMouseDown;
	private Vector2 mPrevMousePos;
	private RectTransform mWheel;
	private Vector3[] wheelCorners = new Vector3[4];
	private float rotationSpeed;
	[SerializeField] private float torque;
	[SerializeField] private float rotationSpeedLimit;
	[SerializeField] private float rotationFriction;

	// Use this for initialization
	void Start ()
	{
		mWheel = this.gameObject.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update ()
	{
		float addedRotationSpeed = calcAddedRotSpeed(mMouseDown, mWheel, mPrevMousePos);
        rotationSpeed = calcRotSpeed(addedRotationSpeed, rotationFriction);		
		mWheel.Rotate (new Vector3 (0f, 0f, rotationSpeed), Space.World);
	}
    
	private float calcAddedRotSpeed (bool pMouseDown, RectTransform pWheel, Vector3 pPrevMousePos)
	{
		if (pMouseDown) {			
			// Get corner points of wheel UI object in world space
			pWheel.GetWorldCorners (wheelCorners);

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
			float direction = Vector3.Cross (new Vector3 (pPrevMousePos.x - wheelCenterX, pPrevMousePos.y - wheelCenterY, 0f), 
				                  new Vector3 (Input.mousePosition.x - wheelCenterX, Input.mousePosition.y - wheelCenterY, 0f)).normalized.z;
			
			
			// Store this frame's mouse position, to be used next frame (unless mouse is let up)
			mPrevMousePos = Input.mousePosition;

            // As long as we've moved the mouse from one frame to the next, rotate the wheel
            // according to which direction we rotated. 
            float lAddedRotSpeed = direction * torque * Time.deltaTime;
            return lAddedRotSpeed;
        }
        else
        {
            return 0;
        }
	}

    private float calcRotSpeed(float pAddedRotationSpeed, float pRotationFriction)
    {
        float lRotationSpeed = rotationSpeed * (1 - pRotationFriction) + pAddedRotationSpeed;

        if (Mathf.Abs(rotationSpeed) > rotationSpeedLimit)
        {
            lRotationSpeed = rotationSpeed / Mathf.Abs(rotationSpeed) * rotationSpeedLimit;
        }

        for (int i = 0; i < 4; i++)
        {
            this.transform.GetChild(i).Rotate(new Vector3(0, 0, -lRotationSpeed * 6), Space.World);
        }

        return lRotationSpeed;
    }

	public void OnPointerDown (PointerEventData eventData)
	{
        mMouseDown = true;
        mPrevMousePos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mMouseDown = false;
    }
}

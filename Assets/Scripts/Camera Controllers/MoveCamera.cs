/* Allows the camera to move around while creating a 
 * new level
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] float mMoveSpeed;
    [SerializeField] float mZoomSpeed;
    [SerializeField] private Transform mGrid;
    private Camera mCam;

    /** 
     * Store the camera component on this game object to the mCam reference. 
     */ 
    void Start()
    {
        mCam = GetComponent<Camera>();
    }

    /**
     * Move the camera and zoom the camera based on input
     */ 
    void LateUpdate()
    {
        move();
        zoom();
    }

    /**
     * Move the camera. Use wasd for standalone, and 
     * touch input for anything else. 
     */ 
    private void move()
    {

        float lHorizontal, lVertical;
        float lVerticalLock = mGrid.localScale.y / 2;
        float lHorizontalLock = mGrid.localScale.x / 2;

#if UNITY_STANDALONE
        lHorizontal = Input.GetAxis("Horizontal");
        lVertical = Input.GetAxis("Vertical");
#else
        lHorizontal = 0;
        lVertical = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            bool noUIcontrolsInUse = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            if (noUIcontrolsInUse)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                lHorizontal = -touchDeltaPosition.x;
                lVertical = -touchDeltaPosition.y;
            }
        }

#endif
        move(lHorizontal, lVertical, lVerticalLock, lHorizontalLock, mMoveSpeed);        
    }

    /**
     * Based on some horizontal and vertical inputs, some constraints along the x and y 
     * for how fast a camera can move, and some move speed, move the camera. 
     * If we are outside of the constraints, do nothing. 
     */ 
    private void move(float pHorizontal, float pVertical, float pVerticalConstraint, float pHorizontalConstraint, float pMoveSpeed)
    {
        bool pIsBetweenZeroAndNegVert = pVertical < 0 && transform.position.y > -pVerticalConstraint;
        bool pIsBetweenZeroAndPosVert = pVertical > 0 && transform.position.y < pVerticalConstraint;

        if (pIsBetweenZeroAndNegVert || pIsBetweenZeroAndPosVert)
        {
            transform.Translate(new Vector2(0, pVertical) * pMoveSpeed * Time.deltaTime);
        }

        bool pIsBetweenZeroAndNegHor = pHorizontal < 0 && transform.position.x > -pHorizontalConstraint;
        bool pIsBetweenZeroAndPosHor = pHorizontal > 0 && transform.position.x < pHorizontalConstraint;

        if (pIsBetweenZeroAndNegHor || pIsBetweenZeroAndPosHor)
        {
            transform.Translate(new Vector2(pHorizontal, 0) * pMoveSpeed * Time.deltaTime);
        }
    }

    /**
     * Make camera zoom in/out based on scroll wheel for standalone, or pinching 
     * for anything else
     */
    private void zoom()
    {
        float lZoomInput;
        float lZoomSpeed;

#if UNITY_STANDALONE
        lZoomInput = Input.GetAxis("Mouse ScrollWheel");
        lZoomSpeed = mZoomSpeed;
#else

        lZoomInput = 0;
        lZoomSpeed = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            bool noUIcontrolsInUse = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            if (noUIcontrolsInUse)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                lZoomInput = getAndroidPinchSpeed();
                lZoomSpeed = 0.6f;
            }
        }
#endif
        zoom(lZoomInput, mCam.orthographicSize, lZoomSpeed);
    }

    /**
     * Get the speed of the pinching on an android device. Used to 
     * determine how fast camera should zoom in/out. 
     */ 
    private float getAndroidPinchSpeed()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;            
            return -deltaMagnitudeDiff;
        }
        else
        {
            return 0;
        }
    }

    /**
     * Using some input, distance, and speed parameters
     * determine how much the camera will be zoomed. 
     * If we are outside of certain distance boundaries,
     * do nothing. 
     */ 
    private void zoom(float pZoomInput, float pZoomDistance, float pZoomSpeed)
    {
        const float MIN_ORTHOGRAPHIC_SIZE = 6f;
        const float MAX_ORTHOGRAPHIC_SIZE = 55f;

        bool lCanZoomIn = pZoomDistance > MIN_ORTHOGRAPHIC_SIZE;
        bool lCanZoomOut = pZoomDistance < MAX_ORTHOGRAPHIC_SIZE;

        if ((lCanZoomIn && pZoomInput > 0) || (lCanZoomOut && pZoomInput < 0))
        {
            float lNewOrthographicSize = pZoomDistance - pZoomInput * pZoomSpeed * Time.deltaTime;
            mCam.orthographicSize = lNewOrthographicSize;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WheelSegmentTracker : MonoBehaviour
{
    // PUBLIC
    [HideInInspector] public int currentWheelSegment;

    // SHOW IN INSPECTOR
    [SerializeField] private AudioSource wheelChangeClip;
    [SerializeField] private TMPro.TextMeshProUGUI segmentText;
    
    // OTHER PRIVATE COMPONENTS
    //private RectTransform wheelRect;

    // OTHER PRIVATE DATA MEMBERS


    // ACCESSORS AND MUTATORS
    public int getCurrentWheelSegment()
    {
        return currentWheelSegment;
    }
    private void setCurrentWheelSegment()
    {
        float lSegmentStartAngle = calculateNormalizedWheelAngle(CurrentWheelAngle + .5f * calculateSegmentAngle(NumSections), InitWheelAngle);
        int lSegment = (int)(lSegmentStartAngle) / (int)(calculateSegmentAngle(NumSections));
        currentWheelSegment = lSegment;
    }
    [SerializeField] private int InitWheelAngle { get; set; }
    private float CurrentWheelAngle { get; set; }
    private AudioSource WheelChangeClip { get; set; }
    private RectTransform WheelRect { get; set; }
    private float NumSections { get; set; }
    

    /*
        -> Store wheel information
            -> Wheel is this object
            -> Get rect transform of wheel
            -> Get wheel rotation
            -> Get number of options in wheel
        -> Add each individual options rect transform 
            and image transform to a list
     */
    private void Start()
    {
        Transform wheel = this.transform;
        WheelRect = wheel.GetComponent<RectTransform>();
        CurrentWheelAngle = WheelRect.eulerAngles.z;
        NumSections = wheel.childCount;        

        setCurrentWheelSegment();
        onChangeEvent.Invoke(getCurrentWheelSegment());
    }

    private void Update()
    {
        calculateNormalizedWheelAngle(CurrentWheelAngle, InitWheelAngle);
        checkPassCutoff(CurrentWheelAngle, NumSections);
    }

    // SUPPORT METHODS-------------------------------------------------------------------------------
    public IntegerEvent onChangeEvent;

    /*
        -> Set the angle thresholds by considering how 
            many wheel options there are in the wheel
        -> When you cross over certain angle thresholds,
            -> Play a clicking sound
            -> Change the color of the play button
            -> Update the wheel option
     */
    private void checkPassCutoff(float pLastWheelAngle, float pNumSections)
    {
        const float DEGREES_IN_CIRCLE = 360;
        // Calculate the angle of a segment
        float lSegmentAngle = calculateSegmentAngle(NumSections);

        // Calculate the difference between the current rotation and the rotation 
        // last frame. 
        CurrentWheelAngle = calculateNormalizedWheelAngle(WheelRect.eulerAngles.z, InitWheelAngle);

        // If the last wheel angle and the current wheel angle 
        // were on different sides of a cutoff point, we have 
        // switched to a different segment of the wheel. 
        for (int i = 1; i <= NumSections; i++)
        {
            // 90 - 45 = 45
            // 180 - 45 = 135
            // 270 - 45 = 225
            // 360 - 45 = 315
            float lCutoffAngle = lSegmentAngle * (i - .5f);
            bool lIsPassingCutoffClockwise = CurrentWheelAngle > lCutoffAngle && pLastWheelAngle < lCutoffAngle && CurrentWheelAngle < DEGREES_IN_CIRCLE * .99f;
            bool lIsPassingCutoffCounterclockwise = pLastWheelAngle > lCutoffAngle && CurrentWheelAngle < lCutoffAngle && CurrentWheelAngle > DEGREES_IN_CIRCLE * .01f ;
            if (lIsPassingCutoffClockwise || lIsPassingCutoffCounterclockwise)
            {
                wheelChangeClip.Play();
                setCurrentWheelSegment();
                segmentText.text = this.transform.GetChild(getCurrentWheelSegment()).name;
                onChangeEvent.Invoke(getCurrentWheelSegment());
            }
        }
    }

    private float calculateSegmentAngle(float pNumSections)
    {
        const float DEGREES_IN_CIRCLE = 360;
        return DEGREES_IN_CIRCLE / pNumSections;
    }

    private float calculateNormalizedWheelAngle(float pCurrentWheelAngle, float pInitWheelAngle)
    {
        const float DEGREES_IN_CIRCLE = 360;
        float lNormalizedWheelAngle = pCurrentWheelAngle - pInitWheelAngle;

        while (lNormalizedWheelAngle < 0)
        {
            lNormalizedWheelAngle = lNormalizedWheelAngle + DEGREES_IN_CIRCLE;
        }

        while (lNormalizedWheelAngle > DEGREES_IN_CIRCLE)
        {
            lNormalizedWheelAngle = lNormalizedWheelAngle - DEGREES_IN_CIRCLE;
        }

        return lNormalizedWheelAngle;
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSegmentSizeManager : MonoBehaviour {

    private int currentWheelSegment;
    private List<RectTransform> WheelSegmentRects = new List<RectTransform>();

    // Accessors and Mutators
    private int NumSections { get; set; }
    private int getCurrentWheelSegment()
    {
        return currentWheelSegment;
    }
    public void setCurrentWheelSegment(int pNewWheelSegment)
    {
        currentWheelSegment = pNewWheelSegment;
    }
    [SerializeField] private bool IsMakingCurrentSegmentBigger;

    private void OnEnable()
    {
        foreach (Transform transform in this.transform)
        {
            WheelSegmentRects.Add(transform.GetComponent<RectTransform>());
        }
        NumSections = WheelSegmentRects.Count;        
    }

    private void Update()
    {
        changeWheelSegmentSize(getCurrentWheelSegment(), NumSections, WheelSegmentRects, IsMakingCurrentSegmentBigger);
    }

    private void changeWheelSegmentSize(int pCurrentWheelSegment, int pNumSections, List<RectTransform> pWheelSegmentRects, bool pIsMakingCurrentSegmentBigger)
    {
        if (pIsMakingCurrentSegmentBigger)
        {
            const float lNORMAL_WHEEL_SIZE = 250f;
            const float lBIG_WHEEL_SIZE = 275f;
            const float lLERP_SPEED = 5f;

            for (int i = 0; i < pNumSections; i++)
            {
                Vector2 lLerpedSize;
                if (i == pCurrentWheelSegment)
                {
                    lLerpedSize = Vector2.Lerp(pWheelSegmentRects[i].sizeDelta, new Vector2(lBIG_WHEEL_SIZE, lBIG_WHEEL_SIZE), lLERP_SPEED * Time.deltaTime);
                }
                else
                {
                    lLerpedSize = Vector2.Lerp(pWheelSegmentRects[i].sizeDelta, new Vector2(lNORMAL_WHEEL_SIZE, lNORMAL_WHEEL_SIZE), lLERP_SPEED * Time.deltaTime);
                }
                pWheelSegmentRects[i].sizeDelta = lLerpedSize;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButtonColorManager : MonoBehaviour {

    [SerializeField] private List<Color> wheelSegmentColors = new List<Color>();

    // Accessors and Mutators------------------------------------
    private void setWheelSegmentColors(Transform pWheel)
    {
        foreach (RectTransform  segmentImage in pWheel)
        {
            Color lSegmentImageColor = segmentImage.GetComponent<Image>().color;
            wheelSegmentColors.Add(lSegmentImageColor);
        }
    }

    private List<Color> getWheelSegmentColors()
    {
        return wheelSegmentColors;
    }

    // Monobehaviour Methods-------------------------------------
    // We need to set the color on awake, because the changeConfirmButtonColor gets called on Start.
    private void Awake()
    {
        setWheelSegmentColors(GameObject.Find("Wheel").transform);
    }

    // Other Interface Methods-----------------------------------
    public void changeConfirmButtonColor(int pCurrentWheelSegment)
    {
        changeConfirmButtonColor(pCurrentWheelSegment, this.GetComponent<Button>(), getWheelSegmentColors());
    }

    // Support Methods-------------------------------------------
    private void changeConfirmButtonColor(int pCurrentWheelSegment, Button pConfirmButton, List<Color> pWheelSegmentColors)
    {
        ColorBlock lConfirmButtonColors = pConfirmButton.colors;

        lConfirmButtonColors.normalColor = pWheelSegmentColors[pCurrentWheelSegment];
        lConfirmButtonColors.highlightedColor = pWheelSegmentColors[pCurrentWheelSegment] + new Color(0, 0, 0, -.4f);
        pConfirmButton.colors = lConfirmButtonColors;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBasedOnWheel : MonoBehaviour
{
    // PUBLIC
    [HideInInspector] public int currentWheelOption;

    // SHOW IN INSPECTOR
    [SerializeField] private bool wheelOptionsChangeSize;
    [SerializeField] private int startAngle;
    [SerializeField] private Button confirmButton;
    [SerializeField] private AudioSource wheelChangeClip;

    // PRIVATE OBJECT 
    private Transform wheel;

    // PRIVATE COMPONENTS
    private List<RectTransform> wheelOptionRects = new List<RectTransform>();
    private List<Color> wheelOptionColors = new List<Color>();
    private RectTransform wheelRect;

    // PRIVATE
    private float rotation;
    private int numSections;
    private const int DEGREES_IN_CIRCLE = 360;

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
        wheel = this.transform;
        wheelRect = wheel.GetComponent<RectTransform>();
        rotation = wheelRect.eulerAngles.z;
        numSections = wheel.childCount;

        for (int i = 0; i < numSections; i++)
        {
            Transform wheelOption = wheel.GetChild(i);
            wheelOptionRects.Add(wheelOption.GetComponent<RectTransform>());
            wheelOptionColors.Add(wheelOption.GetComponent<Image>().color);
        }

        UpdateWheelOption();
        ChangePlayButtonColor();
    }

    private void Update()
    {
        PassAngleThreshold();
        ChangeWheelSize();
    }

    /*
        -> Set the angle thresholds by considering how 
            many wheel options there are in the wheel
        -> When you cross over certain angle thresholds,
            -> Play a clicking sound
            -> Change the color of the play button
            -> Update the wheel option
     */
    private void PassAngleThreshold()
    {
        float lastRotation = rotation;
        rotation = wheelRect.eulerAngles.z;

        float difference = rotation - lastRotation;
        float cutPoint = DEGREES_IN_CIRCLE / numSections;

        for (int i = 1; i < 5; i++)
        {
            if ((rotation > cutPoint * i && lastRotation < cutPoint * i) ||
            (lastRotation > cutPoint * i && rotation < cutPoint * i))
            {
                wheelChangeClip.Play();
                UpdateWheelOption();
                ChangePlayButtonColor();
            }
        }
    }

    /*
        -> Based on the rotation of the wheel, 
            store the right wheel option.
     */
    private void UpdateWheelOption()
    {
        float rotationAmount = rotation - startAngle;

        if (rotationAmount < 0)
        {
            rotationAmount = rotationAmount + DEGREES_IN_CIRCLE;
        }

        while (rotationAmount > DEGREES_IN_CIRCLE)
        {
            rotationAmount = rotationAmount - DEGREES_IN_CIRCLE;
        }

        currentWheelOption = ((int)(rotationAmount)) / (int)(DEGREES_IN_CIRCLE / numSections);
    }

    /*
        -> Change the color of the play button 
            according to which wheel option is 
            currently selected
     */
    private void ChangePlayButtonColor()
    {
        ColorBlock color = confirmButton.colors;
        color.highlightedColor = wheelOptionColors[currentWheelOption];
        confirmButton.colors = color;
    }

    /*
        -> If you want (check the box in inspector), 
            change the size of the currently selected 
            wheel option to be bigger, and make all 
            others the regular size. 
            -> Lerp to the sizes over time. 
     */
    private void ChangeWheelSize()
    {
        if (wheelOptionsChangeSize)
        {
            float normalWheelSize = 250f;
            float bigWheelSize = 285f;

            for (int i = 0; i < numSections; i++)
            {
                if (i == currentWheelOption)
                {
                    Vector2 lerpedSize = Vector2.Lerp(wheelOptionRects[i].sizeDelta, new Vector2(bigWheelSize, bigWheelSize), 5f * Time.deltaTime);
                    wheelOptionRects[i].sizeDelta = lerpedSize;
                }
                else
                {
                    Vector2 lerpedSize = Vector2.Lerp(wheelOptionRects[i].sizeDelta, new Vector2(normalWheelSize, normalWheelSize), 5f * Time.deltaTime);
                    wheelOptionRects[i].sizeDelta = lerpedSize;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSkillsPane : MonoBehaviour {

    [SerializeField] private Button skillPaneToggler;
    [SerializeField] private RectTransform sideBar;
    [SerializeField] private RectTransform topBar;
    private bool isSkillPaneOpen = false;


	public void toggleSkillPane()
    {
        if (isSkillPaneOpen == false)
        {
            sideBar.anchoredPosition = Vector3.zero;
            topBar.anchoredPosition = new Vector3(180f, 0f, 0f);
            skillPaneToggler.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            skillPaneToggler.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            isSkillPaneOpen = true;
        }
        else
        {
            sideBar.anchoredPosition = new Vector3(-180f, 0f, 0f);
            topBar.anchoredPosition = Vector3.zero;
            skillPaneToggler.GetComponent<RectTransform>().localScale = new Vector3(-1f, 1f, 1f);
            skillPaneToggler.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
            isSkillPaneOpen = false;
        }
    }
}

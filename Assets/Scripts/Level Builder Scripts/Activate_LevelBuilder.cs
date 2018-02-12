using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Activate_LevelBuilder : MonoBehaviour
{
    [SerializeField] private Transform wheel;
    [SerializeField] private Transform saveBuildingBlockPackage;
    [SerializeField] private Transform createLevelPackage;
    [SerializeField] private Transform deleteBuildingBlockPackage;
    [SerializeField] private Animator animator;
    [SerializeField] private Image black;
	[SerializeField] private Text warning;


    private UpdateBasedOnWheel wheelUpdater;
    private MoveCamera moveCamera;
    private Transform template;

    // Use this for initialization
    void Start()
    {
        wheelUpdater = wheel.GetComponent<UpdateBasedOnWheel>();
        moveCamera = Camera.main.GetComponent<MoveCamera>();
        template = GameObject.Find("Template").transform;
    }

    public void PerformActivate()
    {
        moveCamera.enabled = false;
        wheel.parent.gameObject.SetActive(false);
        template.gameObject.SetActive(false);

        switch (wheelUpdater.currentWheelOption)
        {
            case 0:
                saveBuildingBlockPackage.gameObject.SetActive(true);
                break;
            case 1:
                createLevelPackage.gameObject.SetActive(true);
                break;
            case 2:
                wheel.parent.gameObject.SetActive(true);
                StartCoroutine(Fading());
                break;
            case 3:
                deleteBuildingBlockPackage.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator Fading()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene("Main Menu");
    }

    public void PerformDeactivate()
    {
        moveCamera.enabled = true;
        wheel.parent.gameObject.SetActive(true);
        template.gameObject.SetActive(true);


        switch (wheelUpdater.currentWheelOption)
        {
            case 0:
                saveBuildingBlockPackage.gameObject.SetActive(false);
				warning.color = new Color(warning.color.r, warning.color.g, warning.color.b, 0);
                break;
            case 1:
                createLevelPackage.gameObject.SetActive(false);
                break;
            case 2:
                break;
            case 3:
                deleteBuildingBlockPackage.gameObject.SetActive(false);
                break;
        }
    }
}

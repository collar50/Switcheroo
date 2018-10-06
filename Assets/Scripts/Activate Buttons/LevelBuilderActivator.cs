using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBuilderActivator : MonoBehaviour
{
    [SerializeField] private Transform mWheel;
    [SerializeField] private Transform mSaveBuildingBlockPackage;
    [SerializeField] private Transform mDeleteBuildingBlockPackage;
    [SerializeField] private Transform mCreateLevelPackage;
    [SerializeField] private Image mBlackScreen;
	[SerializeField] private Text mWarningText;


    private WheelSegmentTracker mWheelSegmentTracker;
    private MoveCamera mCameraMover;
    private Transform mTemplate;

    // Use this for initialization
    void Start()
    {
        mWheelSegmentTracker = mWheel.GetComponent<WheelSegmentTracker>();
        mCameraMover = Camera.main.GetComponent<MoveCamera>();
        mTemplate = GameObject.Find("Template").transform;
    }
             
    public void confirmChoice()
    {
        activateNormalPackage(mCameraMover, mWheel, mTemplate, false);
        activateChoicePackage(mWheelSegmentTracker, mSaveBuildingBlockPackage, mDeleteBuildingBlockPackage, mCreateLevelPackage, mWarningText, mBlackScreen, true);
    }

    public void deconfirmChoice()
    {
        activateNormalPackage(mCameraMover, mWheel, mTemplate, true);
        activateChoicePackage(mWheelSegmentTracker, mSaveBuildingBlockPackage, mDeleteBuildingBlockPackage, mCreateLevelPackage, mWarningText, mBlackScreen, false);
    }

    private void activateChoicePackage(WheelSegmentTracker pWheelSegmentTracker, Transform pSaveBuildingBlockPackage, 
                                        Transform pDeleteBuildingBlockPackage, Transform pCreateLevelPackage,
                                        Text pWarningText, Image pBlackScreen, bool pIsActivating)
    {      
        switch (pWheelSegmentTracker.currentWheelSegment)
        {
            case 0:
                pSaveBuildingBlockPackage.gameObject.SetActive(pIsActivating);
                if (!pIsActivating)
                {
                    pWarningText.color = new Color(pWarningText.color.r, pWarningText.color.g, pWarningText.color.b, 0);
                }
                break;
            case 1:
                pDeleteBuildingBlockPackage.gameObject.SetActive(pIsActivating);
                break;
            case 2:
                pCreateLevelPackage.gameObject.SetActive(pIsActivating);
                break;
            case 3:
                if (pIsActivating)
                {
                    mWheel.parent.gameObject.SetActive(true);
                    IEnumerator fade = fadeToMainMenu(pBlackScreen);
                    StartCoroutine(fade);
                }
                break;
        }
    }

    private void activateNormalPackage(MoveCamera pCameraMover, Transform pWheel, Transform pTemplate, bool pIsActivating)
    {
        pCameraMover.enabled = pIsActivating;
        pWheel.parent.gameObject.SetActive(pIsActivating);
        pTemplate.gameObject.SetActive(pIsActivating);

        if (pIsActivating)
        {
            pTemplate.GetComponent<TemplateController>().updateBuildingBlockSelector();
        }
    }

    IEnumerator fadeToMainMenu(Image pBlackScreen)
    {
        const string NEW_SCENE_NAME = "Main Menu";
        const string ANIMATION_NAME = "Fade";
        const bool IS_ANIMATION_TRIGGERED = true;
        const float MAX_ALPHA = 1;

        Animator lFadeToBlackAnimator = pBlackScreen.GetComponent<Animator>();

        lFadeToBlackAnimator.SetBool(ANIMATION_NAME, IS_ANIMATION_TRIGGERED);
        yield return new WaitUntil(() => pBlackScreen.color.a == MAX_ALPHA);
        SceneManager.LoadScene(NEW_SCENE_NAME);
    }

}

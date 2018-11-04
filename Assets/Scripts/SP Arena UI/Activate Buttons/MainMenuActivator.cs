using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuActivator : MonoBehaviour
{
	[SerializeField] private Image mBlackScreen; // May be able to just hard code this. 
    [SerializeField] private WheelSegmentTracker mWheelSegmentTracker;
    [SerializeField] private string[] mSceneNames;

    // Start the process of changing to a new scene based on the current wheel segment.
    public void confirm()
    {
        IEnumerator fade = fadeToNewScene(mBlackScreen, mSceneNames, mWheelSegmentTracker);
        StartCoroutine(fade);
    }

    // Load a new scene based on the current wheel segment. 
    private void loadNewScene (string[] pSceneNames, WheelSegmentTracker pWheelSegmentTracker)
	{
		string lSceneName = pSceneNames [pWheelSegmentTracker.currentWheelSegment];
		SceneManager.LoadScene (lSceneName);
	}

    // Start the fade-to-black animation, and then change to a new scene based on the current wheel segment. 
	IEnumerator fadeToNewScene(Image pBlackScreen, string[] pSceneNames, WheelSegmentTracker pWheelSegmentTracker){
        const string ANIMATION_NAME = "Fade";
        const bool IS_ANIMATION_TRIGGERED = true;
        const float MAX_ALPHA = 1;

        Animator lFadeToBlackAnimator = pBlackScreen.GetComponent<Animator>();
		lFadeToBlackAnimator.SetBool(ANIMATION_NAME, IS_ANIMATION_TRIGGERED);

        // Wait until the alpha on the black screen is 1, then change scene. 
		yield return new WaitUntil(()=>pBlackScreen.color.a == MAX_ALPHA);
		loadNewScene(pSceneNames, pWheelSegmentTracker);
	}


}

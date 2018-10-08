using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerActivator : MonoBehaviour {

    [SerializeField] private Transform mWheel;
    WheelSegmentTracker mWheelSegmentTracker;



	// Use this for initialization
	void Start () {
        mWheelSegmentTracker = mWheel.GetComponent<WheelSegmentTracker>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // @TODO set up high scores display and stats display. 
    public void confirmChoice()
    {
        // If current wheel segment is 0, play

        // If current wheel segment is 1, high scores

        // If current wheel segment is 2, stats

        // If current wheel segment is 3, back to main

        switch (mWheelSegmentTracker.currentWheelSegment)
        {
            case 0:
                SceneManager.LoadScene("Single Player Arena");
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                SceneManager.LoadScene("Main Menu");
                break;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCam : MonoBehaviour {

    [SerializeField] Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (target == null)
        {
            this.GetComponent<MoveCamera>().enabled = true;
            this.enabled = false;
        }
        else
        {
            this.transform.position = target.position + new Vector3(0f, 0f, -5f);
        }
	}
}

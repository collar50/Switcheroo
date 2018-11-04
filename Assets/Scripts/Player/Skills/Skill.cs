using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    private Stat stats;
    public bool Active { get; set; }
    public bool Unlocked { get; set; }

	// Use this for initialization
	void Start () {
        stats = this.GetComponent<Stat>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

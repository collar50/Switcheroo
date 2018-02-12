using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persist : MonoBehaviour
{
	[SerializeField] private static Persist p;
	// Use this for initialization
	void Awake ()
	{
		if (!p) {
			p = this;
			DontDestroyOnLoad (this.gameObject);		
		} else {
			DestroyImmediate (this.gameObject);
		}
	}
}

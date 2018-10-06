using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLevels : MonoBehaviour {
	private Dropdown levelSelector;
	private LevelManager level_Storage;

	// Use this for initialization
	void Start () {
		levelSelector = this.GetComponent<Dropdown>();
		level_Storage = GameObject.Find("Persistent Data").GetComponent<LevelManager>();
		levelSelector.ClearOptions();
		levelSelector.AddOptions(level_Storage.Names);
	}
}

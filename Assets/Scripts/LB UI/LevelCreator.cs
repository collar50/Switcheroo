using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour {
    AllLevels mLevelManager;
    Transform mNewBuildingBlock;
    [SerializeField] Text mNewLevelName;
    [SerializeField] TMPro.TextMeshProUGUI mDescription;
    

	private void Start () {
        mLevelManager = GameObject.Find("Persistent Data").GetComponent<AllLevels>();
        mNewBuildingBlock = GameObject.Find("New Building Block").transform;       
	}
	
    public void createNewLevel()
    {
        createNewLevel(
            mLevelManager, 
            mNewBuildingBlock, 
            mNewLevelName.text);
    }

	private void createNewLevel(
        AllLevels pLevelManager, 
        Transform pNewLevel, 
        string pNewLevelName)
    {
        mLevelManager.addLevel(
            mNewBuildingBlock, 
            mNewLevelName.text, 
            mDescription.text);
    }
}

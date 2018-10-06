using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour {
    LevelManager mLevelManager;
    Transform mNewBuildingBlock;
    [SerializeField] Text mNewLevelName;
    
	private void Start () {
        mLevelManager = GameObject.Find("Persistent Data").GetComponent<LevelManager>();
        mNewBuildingBlock = GameObject.Find("New Building Block").transform;       
	}
	
    public void createNewLevel()
    {
        createNewLevel(mLevelManager, mNewBuildingBlock, mNewLevelName.text);
    }

	private void createNewLevel(LevelManager pLevelManager, Transform pNewLevel, string pNewLevelName)
    {
        Debug.Log("New Level Has Been Created. (" + pNewLevelName + ")");
        mLevelManager.addLevel(mNewBuildingBlock, mNewLevelName.text);
    }
}

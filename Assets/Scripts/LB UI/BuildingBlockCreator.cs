using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBlockCreator : MonoBehaviour
{
    [SerializeField] private Text mBuildingBlockName { get; set; }
    [SerializeField] private Animator mTooManyPiecesWarning;
    [SerializeField] private LevelBuilderActivator mLevelBuilderActivator { get; set; }
    
    private Dropdown mBuildingBlockSelector { get; set; }
    private BuildingBlockManager mBuildingBlockManager { get; set; }

    //----------------------------------------------------------------------

    private void Start()
    {
        // UI dropdown used for selecting building blocks
        if (GameObject.Find("Building Block Selector") != null)
        {
            mBuildingBlockSelector = GameObject.Find("Building Block Selector").GetComponent<Dropdown>();
        }

        // Building block manager
        if (GameObject.Find("Persistent Data") != null)
        {
            mBuildingBlockManager = GameObject.Find("Persistent Data").GetComponent<BuildingBlockManager>();
        }

        if (GameObject.Find("Confirm Button") != null)
        {
            mLevelBuilderActivator = GameObject.Find("Confirm Button").GetComponent<LevelBuilderActivator>();
        }

        updateBuildingBlockSelectorList();
    }

    public void updateBuildingBlockSelectorList()
    {
        GameObject lPersistentData = GameObject.Find("Persistent Data");
        updateBuildingBlockSelectorList(mBuildingBlockSelector, lPersistentData , mBuildingBlockManager);
    }

    private void updateBuildingBlockSelectorList(Dropdown pBuildingBlockSelector, GameObject pPersistentData, BuildingBlockManager pBuildingBlockManager)
    {
        pBuildingBlockSelector.ClearOptions();
        // Reference to list of building blocks saved by the player
        if (pPersistentData != null)
        {
            for (int i = 0; i < pBuildingBlockManager.Names.Count; i++)
            {
                string lName = pBuildingBlockManager.Names[i];
                string lDisplayName = (i + 1).ToString() + ". " + lName;
                pBuildingBlockSelector.AddOptions(new List<string>() { lDisplayName });
            }
        }
    }
       
    public void createNewBuildingBlock()
    {
        // Building block manager
        if (GameObject.Find("Building Block Name") != null)
        {
            mBuildingBlockName = GameObject.Find("Building Block Name").GetComponent<Text>();
        }
        Transform lNewBuildingBlock = GameObject.Find("New Building Block").transform;
        createNewBuildingBlock(lNewBuildingBlock, mBuildingBlockManager, mBuildingBlockName, mLevelBuilderActivator, mTooManyPiecesWarning);
    }

    private void createNewBuildingBlock(Transform pNewBuildingBlock, BuildingBlockManager pBuildingBlockManager, Text pBuildingBlockName, LevelBuilderActivator pLevelBuilderActivator, Animator pTooManyPiecesWarning)
    {
        const int PIECE_LIMIT = 150;

        if (pNewBuildingBlock.childCount < PIECE_LIMIT)
        {
            pBuildingBlockManager.addBuildingBlock(pNewBuildingBlock, pBuildingBlockName.text);            
            pLevelBuilderActivator.deconfirmChoice();
            updateBuildingBlockSelectorList();
        }
        else
        {
            pTooManyPiecesWarning.Play("Warning", 0);
        }
    }
    
 

    // @TODO Move this method to it's own class, referencing
    // template game object.
    /*
	public void PromptUserToCreateLevel ()
	{
		// Calculate total cost of level
		int totalCost = 0;
		foreach (Transform t in createdStructure) {
			totalCost += t.GetComponent<CostManager> ().Cost;
		}

		prompt.SetActive (true);

		prompt.transform.GetChild (3).GetComponent<Text> ().text = totalCost.ToString ();
	}

	public void AddCreatedLevelToList ()
	{		
		if (Wallet.Coins >= totalCost){
			// Create containers for structure position, rotation, and tag
			levelPositions.Add (new List<Vector3> ());
			levelRotations.Add (new List<Quaternion> ());
			levelTags.Add (new List<string> ());

			// Store the position, rotation, and tag of each element of the structure
			foreach (Transform child in createdStructure) {
					levelPositions [structurePositions.Count - 1].Add (child.localPosition);
					levelRotations [structurePositions.Count - 1].Add (child.rotation);
					levelTags [structurePositions.Count - 1].Add (child.tag);
				}
			}
		}
	}
*/


}

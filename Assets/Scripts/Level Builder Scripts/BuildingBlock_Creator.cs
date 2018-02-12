using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBlock_Creator : MonoBehaviour
{
    // SHOWN IN INSPECTOR
    [SerializeField] private Text bbText;
    [SerializeField] private Animator warning;
    [SerializeField] private Activate_LevelBuilder activate;

    // PRIVATE OBJECTS
    private Dropdown bbSelector;

    // PRIVATE COMPONENTS
    private Transform currentConstruction;
    private BuildingBlock_Storage bbStorage;


    /*
		-> Updates bbSelector according to the data in 
		bbStorage. 
	 */
    public void UpdateBbSelector()
    {
        bbSelector.ClearOptions();
        // Reference to list of building blocks saved by the player
        if (GameObject.Find("Persistent Data") != null)
        {
            for (int i = 0; i < bbStorage.Names.Count; i++)
            {
                string name = bbStorage.Names[i];
                AddNameToBbSelector(name, i);
            }
        }
    }

    /*
    	-> Create a new building block
     */
    public void Create()
    {
        Debug.Log("In Create");
        if (currentConstruction.childCount < 150)
        {
            AddNameToBbStorage(bbText.text, bbStorage.Positions.Count);
            bbStorage.Add(currentConstruction);
            activate.PerformDeactivate();
        }
        else
        {
            Debug.Log("Should play warning");
            warning.Play("Warning", 0);

        }
    }

    /*
		-> Initialize references
		-> Run UpdateBbSelector once to initialize for built-in prefabs. 
	 */
    private void Start()
    {
        // Container for all of the game pieces already placed down
        if (GameObject.Find("Current Construction") != null)
        {
            currentConstruction = GameObject.Find("Current Construction").transform;
        }

        // UI dropdown used for selecting building blocks
        if (GameObject.Find("Building Block Selector") != null)
        {
            bbSelector = GameObject.Find("Building Block Selector").GetComponent<Dropdown>();
        }

        bbStorage = GameObject.Find("Persistent Data").GetComponent<BuildingBlock_Storage>();

        UpdateBbSelector();
    }

    /*
		-> Add a new building block name to bbStorage
		-> Automatically updates bbSelector
	 */
    private void AddNameToBbStorage(string baseName, int index)
    {
        bbStorage.AddName(baseName);
        AddNameToBbSelector(baseName, index);
    }

    /*
        -> Add a building block name to the bbSelector. 
        -> NOTE: We can use this method to totally refresh 
            the bbSelector list based on what already exists
            in bbStorage. This is why we separate it from 
            AddNameToBbStorage. Infact, this is exactly 
			what we do in UpdateBbSelector. 
     */
    private void AddNameToBbSelector(string baseName, int index)
    {
        string decoratedName = (index + 1).ToString() + ") " + baseName;
        bbSelector.AddOptions(new List<string>() { decoratedName });
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

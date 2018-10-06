using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingBlockListManager : MonoBehaviour
{

	private BuildingBlockManager buildingBlockManager;
	private BuildingBlockCreator bbCreator;
	private GameObject dropdownContent;
	private RectTransform dropdownContentRect;
	[SerializeField] Button bbDelButtonPrefab;
	private List<Button> bbDelButtons = new List<Button> ();
	private Text bbDelButtonText;
	private int numBB;
	private string bbName;

	private void OnEnable ()
	{
        Debug.Log("There is a building block list manager component on: " + this.gameObject.name);
		buildingBlockManager = GameObject.Find ("Persistent Data").GetComponent<BuildingBlockManager> ();
		bbCreator = GameObject.Find("UI Manager").GetComponent<BuildingBlockCreator>();

		// "this" is Building Block List gameObject
		dropdownContent = this.transform.GetChild (0).GetChild (0).gameObject;
		dropdownContentRect = dropdownContent.GetComponent<RectTransform> ();
		
		RepopDelBbList ();	
	}

	// Use this for initialization
	private void Update ()
	{
        Debug.Log("There is a building block list manager component on: " + this.gameObject.name);
        ClampContent ();
	}

	/*  -> Repopulate the list and dropdown menu with all of the user-created building blocks in bbStorage.

		-> Ext. reference bbStorage
	*/
	private void RepopDelBbList ()
	{
		ClearBbList();
		// Get the number of building blocks and the number of prefabs. The 
		// number of user generated building blocks will be the first number minus 
		// the second.
		int numPrefabs = buildingBlockManager.BuiltInPrefabs.Count;
		numBB = buildingBlockManager.Names.Count;
		// Instantiate a button for each user-generated building block. Make the button
		// active, set it's position, set it's name, set it's text, and add it to the list of buttons. 
		for (int i = numPrefabs; i < numBB; i++) {
			Button bbButton = Instantiate (bbDelButtonPrefab, dropdownContent.transform.position, transform.rotation, dropdownContent.transform) as Button;
			bbButton.gameObject.SetActive (true);
			bbButton.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, 100f - 50f * (i - numPrefabs));
			bbButton.gameObject.name = "Button" + i.ToString ();
			bbButton.GetComponentInChildren<Text> ().text = (i + 1 - numPrefabs).ToString () + ". " + buildingBlockManager.Names [i];
			bbDelButtons.Add (bbButton);
		}
	}

	/*
		-> Clear list of buttons that represent deletable building blocks. 
		-> Clear dropdown menu that displays this list. 
	*/
	private void ClearBbList(){
		bbDelButtons.Clear ();
		foreach (Transform child in dropdownContent.transform) {
			Destroy (child.gameObject);
		}
	}

	/*
		-> Get building block index using name of button pressed.
		-> Remove building block from bbStorage. 
		-> Repopulate the list of deletable building blocks.
		-> Refill the dropdown with this list. 

		-> Ext. Reference == bbStorage
	 */
	public void DeleteBuildingBlock ()
	{				
		if (bbName != null){
			int index;
			Int32.TryParse (bbName.Substring (6, bbName.Length - 6), out index);

			buildingBlockManager.removeBuildingBlock (index);
			RepopDelBbList ();
		}

		// This updates the list of all building blocks to choose
		// from when creating a level. I could create an event here, 
		// and set up a listener in bbCreator

		bbCreator.updateBuildingBlockSelectorList ();
	}

	/*
		-> Gets the name of the currently selected building block button
	*/
	public void SelectBuildingBlock ()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
		if (currentSelected.tag == "BuildingBlockDeleteButton"){
			bbName = currentSelected.name;
		}
	}

	/*
		-> Control how far the user can scroll through the dropdown
			menu to view all of the buttons for deleting building blocks. 
	 */
	private void ClampContent ()
	{
		float yPos = dropdownContentRect.anchoredPosition.y;

		// Don't let the y-position of the dropdown content rectangle be less 
		// than zero. This restricts how far the user can scroll up. 
		if (yPos < 0f) {
			dropdownContentRect.anchoredPosition = new Vector2 (0f, 0f);
		}

		// Don't let the y-position of the dropdown content rectangle be greater than 
		// what it needs to be to allow the user to see all of the buttons. If there
		// aren't enough buttons to fill up the entire content rectangle, don't allow 
		// scrolling. 
		float topPosition = (float)(numBB - 1) * (50f) - 200f;
		if (yPos > topPosition) {
			if (topPosition < 0) {
				dropdownContentRect.anchoredPosition = new Vector2 (0f, 0f);
			} else {
				dropdownContentRect.anchoredPosition = new Vector2 (0f, topPosition);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlock_Storage : MonoBehaviour
{

	private List<List<Vector3>> positions = new List<List<Vector3>> ();
	private List<List<Quaternion>> rotations = new List<List<Quaternion>> ();
	private List<List<string>> tags = new List<List<string>> ();
	private List<string> names = new List<string> ();
	[SerializeField] private List<Transform> builtInPrefabs;

	// Accessors ----------------------------

	public List<List<Vector3>> Positions { get { return positions; } }

	public List<List<Quaternion>> Rotations { get { return rotations; } }

	public List<List<string>> Tags { get { return tags; } }

	public List<string> Names { get { return names; } }

	public List<Transform> BuiltInPrefabs { get { return builtInPrefabs; } }

	// -----------------------------------------


	private void Start ()
	{
		// Load from save
		LoadAll ();

		if (positions.Count == 0) {
			foreach (Transform t in builtInPrefabs) {
				Add (t);
				AddName (t.gameObject.name);
			}
		}
	}

	private void SaveAll ()
	{

	}

	private void LoadAll ()
	{

	}

	// Add structure to the dropdown list. Invoke this method when user
	// pushes create structure button
	public void Add (Transform currentConstruction)
	{

		int buildingBlock_Index = positions.Count;

		// Create containers for structure position, rotation, and tag
		positions.Add (new List<Vector3> ());
		rotations.Add (new List<Quaternion> ());
		tags.Add (new List<string> ());

		// Store the position, rotation, and tag of each element of the structure
		if (currentConstruction.childCount == 0) {
			positions [buildingBlock_Index].Add (currentConstruction.localPosition);
			rotations [buildingBlock_Index].Add (currentConstruction.rotation);
			tags [buildingBlock_Index].Add (currentConstruction.tag);
		} else {

			// There's probably a much better way to do this, but I 
			// don't know it yet. 
			float highestX = -10000;
			float highestY = -10000;
			float lowestX = 10000;
			float lowestY= 10000;
			foreach(Transform piece in currentConstruction){
				if (piece.localPosition.x > highestX){
					highestX = piece.localPosition.x;
				}
				if (piece.localPosition.x <lowestX){
					lowestX = piece.localPosition.x;
				}
				if (piece.localPosition.y > highestY){
					highestY = piece.localPosition.y;
				}
				if (piece.localPosition.y < lowestY){
					lowestY = piece.localPosition.y;
				}
			}

			float centerX = (float)(Mathf.Round((int)((highestX+lowestX))/8)*4);
			float centerY = (float)(Mathf.Round((int)((highestY+lowestY))/8)*4);

			Debug.Log("Center X: " + centerX + "\nCenter Y: " + centerY);

			foreach (Transform piece in currentConstruction) {
				positions [buildingBlock_Index].Add (piece.localPosition - new Vector3(centerX, centerY, 0f));
				rotations [buildingBlock_Index].Add (piece.rotation);
				tags [buildingBlock_Index].Add (piece.tag);
			}
		}
	}

	// If click an x next to a structure name in the dropdown menu,
	// remove that structure from the list.
	public void Remove (int buildingBlock_Index)
	{
		//@TODO make a way to select the right structure to remove
		positions.RemoveAt (buildingBlock_Index);
		rotations.RemoveAt (buildingBlock_Index);
		tags.RemoveAt (buildingBlock_Index);
		names.RemoveAt (buildingBlock_Index);
		//options.RemoveAt (0);
	}

	// Redundant?
	public void AddName (string name)
	{
		names.Add (name);
	}
}

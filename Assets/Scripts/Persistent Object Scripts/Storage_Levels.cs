using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage_Levels : MonoBehaviour
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

		int buildingBlock_Index = Positions.Count;

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
			foreach (Transform piece in currentConstruction) {
				positions [buildingBlock_Index].Add (piece.localPosition);
				rotations [buildingBlock_Index].Add (piece.rotation);
				tags [buildingBlock_Index].Add (piece.tag);
			}
		}
	}

	// If click an x next to a structure name in the dropdown menu,
	// remove that structure from the list.
	public void Remove (int level_Index)
	{
		//@TODO make a way to select the right structure to remove
		positions.RemoveAt (level_Index);
		rotations.RemoveAt (level_Index);
		tags.RemoveAt (level_Index);
		names.RemoveAt (level_Index);
		//options.RemoveAt (0);
	}

	// Redundant?
	public void AddName (string name)
	{
		names.Add (name);
	}
}

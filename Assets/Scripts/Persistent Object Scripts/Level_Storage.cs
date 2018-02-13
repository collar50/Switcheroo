using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Level_Storage : MonoBehaviour
{
	[SerializeField] private RenderTexture rendTex;
	private List<Transform> levels = new List<Transform>();
	private List<string> names = new List<string> ();

	private List<Sprite> previewImages = new List<Sprite>();
	[SerializeField] private List<Transform> builtInPrefabs;

	// Accessors ----------------------------

	public List<Transform> Levels { get { return levels; } }

	public List<string> Names { get { return names; } }

	public List<Transform> BuiltInPrefabs { get { return builtInPrefabs; } }


	// -----------------------------------------


	private void Start ()
	{
		// Load from save
		LoadAll ();

		if (levels.Count == 0) {
			foreach (Transform t in builtInPrefabs) {
				Add (t, t.gameObject.name);
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
	public void Add (Transform currentConstruction, string name)
	{
		levels.Add (currentConstruction);
		names.Add(name);

		GameObject snapCam = GameObject.Find("SnapshotCamera");

		AssetDatabase.CreateAsset(currentConstruction, "Assets/Prefabs/Prebuilt Levels");

		// Converting rect texture to sprite, storing sprite in assets folder
		Texture2D tex2D = new Texture2D(256,256, TextureFormat.RGB24, false);
		tex2D.ReadPixels(new Rect(0, 0, rendTex.width, rendTex.height), 0, 0);
		if (snapCam != null){
			Sprite s = Sprite.Create(tex2D, new Rect(0.0f, 0.0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
			previewImages.Add(s);
			AssetDatabase.CreateAsset(s, "Assets");
		}
	}

	// If click an x next to a structure name in the dropdown menu,
	// remove that structure from the list.
	public void Remove (int level_Index)
	{
		//@TODO make a way to select the right structure to remove
		levels.RemoveAt(level_Index);
		names.RemoveAt (level_Index);
		//options.RemoveAt (0);
	}
}

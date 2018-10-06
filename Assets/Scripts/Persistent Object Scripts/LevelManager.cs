using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private RenderTexture rendTex;
	private List<Transform> levelList = new List<Transform>();
	private List<string> levelNameList = new List<string> ();
	private List<Sprite> previewImageList = new List<Sprite>();
	[SerializeField] private List<Transform> builtInLevelList;
    [SerializeField] private List<Sprite> builtInPreviewImageList = new List<Sprite>();

	// Accessors ----------------------------

	public List<Transform> LevelList { get { return levelList; } }

	public List<string> Names { get { return levelNameList; } }

	public List<Transform> BuiltInLevelList { get { return builtInLevelList; } }


	// -----------------------------------------


	private void Start ()
	{
		// Load from save
		loadAll ();
        
		if (levelList.Count == 0) {
			foreach (Transform t in builtInLevelList) {
				addLevel (t, t.gameObject.name);
			}

            foreach (Sprite s in builtInPreviewImageList)
            {
                previewImageList.Add(s);
            }
		}
	}

	private void saveAll ()
	{

	}

	private void loadAll ()
	{

	}

	// Add structure to the dropdown list. Invoke this method when user
	// pushes create structure button
	public void addLevel (Transform currentConstruction, string name)
	{
        foreach(Transform t in currentConstruction)
        {
            if (t.GetComponent<Collider2D>() != null)
            {
                t.GetComponent<Collider2D>().usedByComposite = false;
            }
        }

		levelList.Add (currentConstruction);
		levelNameList.Add(name);              

        // USE THESE LINES TO MAKE PREBUILT LEVELS
        //PrefabUtility.CreatePrefab("Assets/Prefabs/Prebuilt Levels/" + name + ".prefab", currentConstruction.gameObject);
        //IEnumerator takeSnapshot = takeSnapshotOfNewLevel(name);
        //StartCoroutine(takeSnapshot);
    }

    private IEnumerator takeSnapshotOfNewLevel(string pName)
    {
        yield return new WaitForEndOfFrame();
        // Converting rect texture to sprite, storing sprite in assets folder
        //Texture2D tex2D = new Texture2D(256,256, TextureFormat.RGB24, false);
        RenderTexture.active = rendTex;
        //GameObject.Find("Snapshot Camera").GetComponent<Camera>().Render();
        Texture2D tex2D = new Texture2D(rendTex.width, rendTex.height, TextureFormat.ARGB32, true);
        Graphics.CopyTexture(rendTex, tex2D);
        Rect rect = new Rect(0, 0, rendTex.width, rendTex.height);
        tex2D.ReadPixels(rect, 0, 0, true);

        if (tex2D != null)
        {
            string path = "Assets/Prefabs/Prebuilt Levels/" + pName + ".png";
            Sprite s = Sprite.Create(tex2D, new Rect(0.0f, 0.0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
            previewImageList.Add(s);

            // USE THIS LINE TO MAKE PREBUILT LEVELS
            //File.WriteAllBytes(path, s.texture.EncodeToPNG());
            //File.WriteAllBytes(path, s.texture.EncodeToPNG());
            //AssetDatabase.Refresh();
            //AssetDatabase.AddObjectToAsset(s, path);
            //AssetDatabase.SaveAssets();
        }        
    }
    

	// If click an x next to a structure name in the dropdown menu,
	// remove that structure from the list.
	public void removeLevel (int level_Index)
	{
		//@TODO make a way to select the right structure to remove
		levelList.RemoveAt(level_Index);
		levelNameList.RemoveAt (level_Index);
		//options.RemoveAt (0);
	}
}

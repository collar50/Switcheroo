using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AllLevels : MonoBehaviour
{
	[SerializeField] private RenderTexture rendTex;
    [SerializeField] private List<BuiltInLevel> builtInLevels = new List<BuiltInLevel>();
    private List<CustomLevel> customLevels = new List<CustomLevel>();
    public List<Vector3> currentLevelPositions;
    public List<string> currentLevelPrefabs;
    public int currentLevel;
         
    // Accessors ----------------------------
    public List<BuiltInLevel> BuiltInLevels { get { return builtInLevels; } }
    public List<CustomLevel> CustomLevels { get { return customLevels; } }       
	// -----------------------------------------
    
	private void Start ()
	{
        foreach (BuiltInLevel b in BuiltInLevels)
        {
            b.parsePieces();
        }

		loadAll ();
	}
    
    private void saveAll ()
	{

	}

	private void loadAll ()
	{

	}

	// Add structure to the dropdown list. Invoke this method when user
	// pushes create structure button
	public void addLevel (Transform structure, string name, string description)
	{
        DecomposeColliders(structure); 

        // USE THESE LINES TO MAKE PREBUILT LEVELS        
        IEnumerator addLevelToAssets = addLevelToAssetsFolder(name, description, structure);
        StartCoroutine(addLevelToAssets);
    }

    private void DecomposeColliders(Transform pieces)
    {
        foreach (Transform piece in pieces)
        {
            if (piece.GetComponent<Collider2D>() != null)
            {
                piece.GetComponent<Collider2D>().usedByComposite = false;
            }
        }
    }

    private IEnumerator addLevelToAssetsFolder(string pName, string pDescription, Transform pStructure)
    {
        yield return new WaitForEndOfFrame();
        //RenderTexture.active = rendTex;
        //Texture2D tex2D = new Texture2D(rendTex.width, rendTex.height, TextureFormat.ARGB32, true);
        //Graphics.CopyTexture(rendTex, tex2D);
        //Rect rect = new Rect(0, 0, rendTex.width, rendTex.height);
        //tex2D.ReadPixels(rect, 0, 0, true);

        
        if (true)
        {         
            string builtInLevelPath = "Assets/Prefabs/Prebuilt Levels/" + pName + ".asset";
            string structurePath = "Assets/Resources/" + pName + ".prefab";
                      
            // Add New Custom Level To List
            customLevels.Add(new CustomLevel(pName, pDescription, pStructure));
            
            // USE THIS FOR CREATING PREBUILT LEVELS
            // Create structure prefab
            UnityEditor.PrefabUtility.CreatePrefab(
                structurePath, 
                pStructure.gameObject);

            // Load structure prefab
            //Transform structureToLoad = Resources.Load<Transform>(structurePath);
            GameObject structureToLoad = Resources.Load<GameObject>(pName);
            Debug.Log(structureToLoad);
            // Construct built in level
            BuiltInLevel builtInLevel = ScriptableObject.CreateInstance<BuiltInLevel>();
            builtInLevel.initializeLevel(pName, pDescription, structureToLoad.transform);
            AssetDatabase.Refresh();
            AssetDatabase.CreateAsset(builtInLevel, builtInLevelPath);
            AssetDatabase.SaveAssets();
            
        }              
    }
    

	// If click an x next to a structure name in the dropdown menu,
	// remove that structure from the list.
	public void removeLevel (int level_Index)
	{
		//@TODO make a way to select the right structure to remove
        
		//options.RemoveAt (0);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlockManager : MonoBehaviour
{
    public List<List<Vector3>> Positions { get; set; }

    public List<List<Quaternion>> Rotations { get; set; }

    public List<List<string>> Tags { get; set; }

    public List<string> Names { get; set; }

    [SerializeField] public List<Transform> BuiltInPrefabs;

    // -----------------------------------------


    private void Start()
    {
        Positions = new List<List<Vector3>>();
        Rotations = new List<List<Quaternion>>();
        Tags = new List<List<string>>();
        Names = new List<string>();

        // Load built in building blocks
        loadBuiltInPrefabs(BuiltInPrefabs, Positions, Rotations, Tags, Names);
        // Load from save
        loadAll();
    }

    private void saveAll()
    {

    }

    private void loadAll()
    {

    }
    
    private void loadBuiltInPrefabs(List<Transform> pBuiltInPrefabs, List<List<Vector3>> pPositions, List<List<Quaternion>> pRotations, List<List<string>> pTags, List<string> pNames)
    {
        if (pPositions.Count == 0)
        {
            foreach (Transform t in pBuiltInPrefabs)
            {
                addBuildingBlock(t, t.gameObject.name, pPositions, pRotations, pTags, pNames);
            }
        }
    }

    public Transform searchBuiltInPrefabs(string pTag)
    {
        // @TODO Linear search
        foreach (Transform t in BuiltInPrefabs)
        {
            if (t.gameObject.tag == pTag)
            {
                return t;
            }
        }

        return null;
    }

    public void addBuildingBlock(Transform pNewBuildingBlock, string pName)
    {
        addBuildingBlock(pNewBuildingBlock, pName, Positions, Rotations, Tags, Names);
    }

    // Add structure to the dropdown list. Invoke this method when user
    // pushes create new building block button
    public void addBuildingBlock(Transform pNewBuildingBlock, string pName, List<List<Vector3>> pPositions, List<List<Quaternion>> pRotations, List<List<string>> pTags, List<string> pNames)
    {

        int lNewBuildingBlockIndex = pPositions.Count;

        // Create containers for new building block pieces positions, rotations, and tags
        pPositions.Add(new List<Vector3>());
        pRotations.Add(new List<Quaternion>());
        pTags.Add(new List<string>());
        pNames.Add(pName);

        // Store the position, rotation, and tag of each element of the structure

        // If a building block contains 1 piece, then that piece IS the building block. Add it. 
        if (pNewBuildingBlock.childCount == 0)
        {
            pPositions[lNewBuildingBlockIndex].Add(pNewBuildingBlock.localPosition);
            pRotations[lNewBuildingBlockIndex].Add(pNewBuildingBlock.rotation);
            pTags[lNewBuildingBlockIndex].Add(pNewBuildingBlock.tag);
        }
        else
        {

            // Get the highest and lowest pieces along both the x and y axes. 
            float highestX = -Mathf.Infinity;
            float highestY = -Mathf.Infinity;
            float lowestX = Mathf.Infinity;
            float lowestY = Mathf.Infinity;
            foreach (Transform lPiece in pNewBuildingBlock)
            {
                if (lPiece.localPosition.x > highestX)
                {
                    highestX = lPiece.localPosition.x;
                }
                if (lPiece.localPosition.x < lowestX)
                {
                    lowestX = lPiece.localPosition.x;
                }
                if (lPiece.localPosition.y > highestY)
                {
                    highestY = lPiece.localPosition.y;
                }
                if (lPiece.localPosition.y < lowestY)
                {
                    lowestY = lPiece.localPosition.y;
                }
            }

            // Calculate the center as the (highest plus the lowest) / 2.
            float centerX = (float)(Mathf.Round((int)((highestX + lowestX)) / 2));
            float centerY = (float)(Mathf.Round((int)((highestY + lowestY)) / 2));
            

            // Add the position, rotation, and tag of each piece to the lists. 
            foreach (Transform lPiece in pNewBuildingBlock)
            {
                pPositions[lNewBuildingBlockIndex].Add(lPiece.localPosition - new Vector3(centerX, centerY, 0f));
                pRotations[lNewBuildingBlockIndex].Add(lPiece.rotation);
                pTags[lNewBuildingBlockIndex].Add(lPiece.tag);
            }
        }
    }
        
    // If click an x next to a structure name in the dropdown menu,
    // remove that structure from the list.
    public void removeBuildingBlock(int buildingBlock_Index)
    {
        //@TODO make a way to select the right structure to remove
        Positions.RemoveAt(buildingBlock_Index);
        Rotations.RemoveAt(buildingBlock_Index);
        Tags.RemoveAt(buildingBlock_Index);
        Names.RemoveAt(buildingBlock_Index);
        //options.RemoveAt (0);
    }
}

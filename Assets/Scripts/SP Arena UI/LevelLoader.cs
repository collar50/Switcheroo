using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {



	// Use this for initialization
	void Start () {
        AllLevels allLevels = 
            GameObject.Find("Persistent Data").GetComponent<AllLevels>();

        BuildingBlockManager buildingBlockManager = 
            GameObject.Find("Persistent Data").GetComponent<BuildingBlockManager>();

        int currentLevelIndex = allLevels.currentLevel;


        if (currentLevelIndex < allLevels.BuiltInLevels.Count) {
            BuiltInLevel currentLevel = allLevels.BuiltInLevels[currentLevelIndex];

            for (int i = 0; i < currentLevel.Prefabs.Count; i++)
            {
                Vector3 piecePosition = currentLevel.PiecePositions[i];

                Quaternion pieceRotation =
                    Quaternion.Euler(
                        0f,
                        0f,
                        currentLevel.PieceRotations[i]);

                string tag = currentLevel.Prefabs[i];

                Instantiate(
                    buildingBlockManager.searchBuiltInPrefabs(tag), 
                    piecePosition, 
                    pieceRotation);
            }
        }
        else if (currentLevelIndex >= allLevels.BuiltInLevels.Count)
        {
            CustomLevel currentLevel = allLevels.CustomLevels[currentLevelIndex];

            for (int i = 0; i < currentLevel.Prefabs.Count; i++)
            {
                Vector3 piecePosition = currentLevel.PiecePositions[i];

                Quaternion pieceRotation = 
                    Quaternion.Euler(
                        0f, 
                        0f, 
                        currentLevel.PieceRotations[i]);

                string tag = currentLevel.Prefabs[i];

                Instantiate(
                    buildingBlockManager.searchBuiltInPrefabs(tag), 
                    piecePosition, 
                    pieceRotation);
            }
        }
    }
}

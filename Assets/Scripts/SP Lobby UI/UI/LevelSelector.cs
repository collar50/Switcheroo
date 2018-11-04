using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
	[SerializeField] private Dropdown levelDropdown;
	private AllLevels AllLevels;
    [SerializeField] private Image preview;
    [SerializeField] private TMPro.TextMeshProUGUI name;
    [SerializeField] private TMPro.TextMeshProUGUI description;

	// Use this for initialization
	void OnEnable () {
		AllLevels = GameObject.Find("Persistent Data")
                        .GetComponent<AllLevels>();


		levelDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (BuiltInLevel level in AllLevels.BuiltInLevels)
        {
            options.Add(level.Name);
        }
        foreach (CustomLevel level in AllLevels.CustomLevels)
        {
            options.Add(level.Name);
        }
        levelDropdown.AddOptions(options);

        levelDropdown.value = AllLevels.currentLevel;
        SelectLevel();
	}

    public void SelectLevel()
    {
        int selection = levelDropdown.value;
        AllLevels.currentLevel = selection;

        if (selection < AllLevels.BuiltInLevels.Count)
        {
            BuiltInLevel selectedLevel = AllLevels.BuiltInLevels[selection];
            
            preview.preserveAspect = true;

            name.text = selectedLevel.Name;
            description.text = selectedLevel.Description;
            AllLevels.currentLevelPositions = selectedLevel.PiecePositions;
            AllLevels.currentLevelPrefabs = selectedLevel.Prefabs;           
        }
        else
        {            
            CustomLevel selectedLevel = AllLevels.CustomLevels[selection - AllLevels.BuiltInLevels.Count];
            
            preview.preserveAspect = true;

            name.text = selectedLevel.Name;
            description.text = selectedLevel.Description;
            AllLevels.currentLevelPositions = selectedLevel.PiecePositions;
            AllLevels.currentLevelPrefabs = selectedLevel.Prefabs;
        }

        ChangeLevel();
    }

    public void ChangeLevel()
    {
        AllLevels allLevels =
            GameObject.Find("Persistent Data").GetComponent<AllLevels>();

        BuildingBlockManager buildingBlockManager =
            GameObject.Find("Persistent Data").GetComponent<BuildingBlockManager>();

        Transform levelContainer = GameObject.Find("Level Container").transform;
        foreach (Transform child in levelContainer)
        {
            Destroy(child.gameObject);
        }

        int currentLevelIndex = allLevels.currentLevel;

        
        if (currentLevelIndex < allLevels.BuiltInLevels.Count)
        {
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

                Transform piece = Instantiate(
                    buildingBlockManager.searchBuiltInPrefabs(tag),
                    piecePosition,
                    pieceRotation);

                piece.parent = levelContainer;
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

                Transform piece = Instantiate(
                    buildingBlockManager.searchBuiltInPrefabs(tag),
                    piecePosition,
                    pieceRotation);

                piece.parent = levelContainer;
            }
        }
    }
}

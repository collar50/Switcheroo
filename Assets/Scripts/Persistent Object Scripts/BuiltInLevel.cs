using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Tools/Levels")]
public class BuiltInLevel : ScriptableObject {
    [SerializeField] private string name;
    [SerializeField][TextArea] private string description;
    [SerializeField] private Transform structure;
    private List<Vector3> piecePositions = new List<Vector3>();
    private List<float> pieceRotations = new List<float>();
    private List<string> prefabs = new List<string>();

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public Transform Structure { get { return structure; } }
    public List<Vector3> PiecePositions { get { return piecePositions; } }
    public List<float> PieceRotations { get { return pieceRotations; } }
    public List<string> Prefabs { get { return prefabs; } }


    
    public void initializeLevel(
        string pName, 
        string pDescription, 
        Transform pTransform)
    {
        name = pName;
        description = pDescription;
        structure = pTransform;

        parsePieces();
    }

    public void parsePieces()
    {
        piecePositions.Clear();
        prefabs.Clear();

        foreach (Transform t in structure)
        {
            piecePositions.Add(t.localPosition);
            pieceRotations.Add(t.eulerAngles.z);
            prefabs.Add(t.gameObject.tag);
        }
    }
}

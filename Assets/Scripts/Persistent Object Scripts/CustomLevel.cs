using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLevel{  
    private string name;
    private string description;
    private List<Vector3> piecePositions = new List<Vector3>();
    private List<float> pieceRotations = new List<float>();
    private List<string> prefabs = new List<string>();
    
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public List<Vector3> PiecePositions { get { return piecePositions; } }
    public List<float> PieceRotations { get { return pieceRotations; } }
    public List<string> Prefabs { get { return prefabs; } }

    public CustomLevel(
        string pName,
        string pDescription,
        Transform pTransform)
    {
        name = pName;
        description = pDescription;

        foreach(Transform t in pTransform)
        {
            piecePositions.Add(t.localPosition);
            pieceRotations.Add(t.eulerAngles.z);
            prefabs.Add(t.gameObject.tag);
        }
    }
}
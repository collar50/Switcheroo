using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : BaseActive
{
	private bool fieldWhileShielded = true;
	[SerializeField] private float boostAmount;
	public List<int> fieldWhileShieldedMultipliers = new List<int> (3);
	public List<int> fieldWhileShieldedTypes = new List<int> (4);
	[SerializeField] Transform fieldPrefab;
	Transform currentField;


	public bool FieldWhileShielded{ set { fieldWhileShielded = value; } }
	// Use this for initialization
	protected override void StartActive ()
	{
		currentField = Instantiate (fieldPrefab, this.transform.position, Quaternion.identity, this.transform);
		stats.fielded = true;
		if (fieldWhileShielded) {
			Debug.Log ("FIELD WHILE SHIELDED");
			fieldWhileShieldedMultipliers.Add (0);
			fieldWhileShieldedMultipliers.Add (1);

			fieldWhileShieldedTypes.Add (0);
			fieldWhileShieldedTypes.Add (1);
			fieldWhileShieldedTypes.Add (2);

			stats.AdjustFieldMultipliers (boostAmount, fieldWhileShieldedMultipliers, fieldWhileShieldedTypes);
		}
		stats.ApplyFieldedMultipliers ();
		stats.SetCurrentToMax ();
	}
	
	// Update is called once per frame
	protected override void EndActive ()
	{
		Destroy (currentField.gameObject);
		currentField = null;
		stats.fielded = false;
		if (fieldWhileShielded) {
			stats.AdjustFieldMultipliers (1f / boostAmount, fieldWhileShieldedMultipliers, fieldWhileShieldedTypes);
		}
		stats.ApplyStartMultipliers ();
		stats.ApplyShieldedMultipliers ();
		stats.SetCurrentToMax ();
	}
}

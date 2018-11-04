using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseActive
{
	private bool shieldWhileFielded = true;
	[SerializeField] private float boostAmount;
	public List<int> shieldWhileFieldedMultipliers = new List<int> (3);
	public List<int> shieldWhileFieldedTypes = new List<int> (4);
	[SerializeField] Transform shieldPrefab;
	Transform currentShield;

	protected override void StartActive ()
	{
		currentShield = Instantiate (shieldPrefab, this.transform.position, Quaternion.identity, this.transform);
		stats.shielded = true;
		if (shieldWhileFielded) {
			Debug.Log ("FIELD WHILE SHIELDED");
			shieldWhileFieldedMultipliers.Add (0);
			shieldWhileFieldedMultipliers.Add (1);

			shieldWhileFieldedTypes.Add (0);
			shieldWhileFieldedTypes.Add (1);
			shieldWhileFieldedTypes.Add (2);

			stats.AdjustFieldMultipliers (boostAmount, shieldWhileFieldedMultipliers, shieldWhileFieldedTypes);
		}
		stats.ApplyFieldedMultipliers ();
		stats.SetCurrentToMax ();
	}

	protected override void EndActive ()
	{
		Destroy (currentShield.gameObject);
		currentShield = null;
		stats.shielded = false;
		if (shieldWhileFielded) {
			stats.AdjustFieldMultipliers (1f / boostAmount, shieldWhileFieldedMultipliers, shieldWhileFieldedTypes);
		}
		stats.ApplyStartMultipliers ();
		stats.ApplyShieldedMultipliers ();
		stats.SetCurrentToMax ();
	}
}

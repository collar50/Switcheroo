using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseActive : MonoBehaviour
{
	private bool alreadyFiredOnce;
	private bool currentlyActive;
	private float startTime;
	private float proportionLeft;
	[SerializeField] private Image image;

	[SerializeField] private float baseCooldown;
	[SerializeField] private float baseDuration;
	[SerializeField] private int baseManaCost;

	private float currentCooldown;
	private float currentDuration;
	private int currentManaCost;

	[HideInInspector] public int cooldownMultiplier = 1;
	[HideInInspector] public int durationMultiplier = 1;
	[HideInInspector] public int manaCostMultiplier = 1;

	protected Stats stats;

	private void Start ()
	{
		startTime = -currentCooldown;
		stats = this.gameObject.GetComponent<Stats> ();
		ApplyMultipliers ();
	}

	private void Update ()
	{
		TrackCooldown ();
		Deactivate ();
	}

	private void ApplyMultipliers ()
	{
		currentCooldown = baseCooldown * (float)cooldownMultiplier;
		currentDuration = baseDuration * (float)durationMultiplier;
		currentManaCost = baseManaCost * manaCostMultiplier;
	}

	// Toggled by button press for player, random for AI
	public void Activate ()
	{
		if (Time.time > startTime + currentCooldown &&
		    alreadyFiredOnce || !alreadyFiredOnce) {
			alreadyFiredOnce = true;
			if (stats.CurrentValue () [1] >= currentManaCost) {
				stats.DecrementCurrentValue (1, currentManaCost);
				startTime = Time.time;
				currentlyActive = true;
				image.fillAmount = 1f;
				StartActive ();
			} else {
				Debug.Log ("NOT ENOUGH MANA!!!");
			}
		} else {
			Debug.Log ("NOT READY YET");
		}
	}

	// Ends naturally after duration ends
	protected void Deactivate ()
	{		
		if (Time.time - startTime > currentDuration && currentlyActive) {
			Debug.Log ("DEACTIVATE");
			currentlyActive = false;
			EndActive ();
		}
	}

	protected void TrackCooldown ()
	{		
		if (image != null &&
		    alreadyFiredOnce &&
		    this.gameObject.tag.Substring (0, 6) == "Player" &&
		    image.fillAmount > 0) {			
			image.fillAmount = 1 - (Time.time - startTime) / currentCooldown;
		}
	}

	protected abstract void StartActive ();

	protected abstract void EndActive ();
}

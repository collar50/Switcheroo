using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
	[SerializeField] private float currentTypeDuration;
    [SerializeField] private Sprite[] verticalSprites;
    [SerializeField] private Sprite[] horizontalSprites;
    [SerializeField] private Sprite[] smallSprites;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    
    public enum WallType { Health, Mana, Switch, Damage};
    [SerializeField] public WallType wallType;

	void Start ()
	{
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        boxCollider = this.GetComponent<BoxCollider2D>();

		changeWallType ();

		// This will be the smallest multiple of the currentTypeDuration that is greater than the timeSinceLevelLoad. It's when, in
		// scene time, the next WallID change should occur. 
		float nextTimeToChange = ((int)Time.timeSinceLevelLoad / (int)currentTypeDuration + 1) * currentTypeDuration;

		// Calibrate the time for the next change to account for when this wall was instantiated
		float timeToChangeSinceThisCreated = nextTimeToChange - Time.timeSinceLevelLoad;

		InvokeRepeating ("changeWallType", timeToChangeSinceThisCreated, currentTypeDuration);
	}

	private void changeWallType ()
	{
        Array wallTypes = Enum.GetValues(typeof(WallType));
        int random = UnityEngine.Random.Range(0, wallTypes.Length);
        wallType = (WallType) wallTypes.GetValue(random);

        setImage();
    }
    
    public void setImage()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        if (this.tag.Equals("LongWall"))
        {
            spriteRenderer.sprite = horizontalSprites[(int)wallType];
            boxCollider.size = new Vector2(2.97f, .97f);
        }
        else
        {
            spriteRenderer.sprite = smallSprites[(int)wallType];
        }
    }
}

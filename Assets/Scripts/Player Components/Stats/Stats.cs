using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// I hate multi-dimensional arrays... can't see in inspector...
public class Stats : MonoBehaviour
{

    public bool shielded;
    public bool fielded;
    private List<Transform> fields;

    [SerializeField] private Transform floatingText;

    /*
	 * 0) Health
	 * 1) Mana
	 * 2) Damage
	 * 3) Switch
	 */

    [SerializeField] private int[] currentValue = new int[4];

    public int[] CurrentValue()
    {
        return currentValue;
    }

    [SerializeField] private int[] baseMaxValue = new int[4];
    private int[] currentMaxValue = new int[4];

    [SerializeField] private int[] baseWallValue = new int[4];
    private int[] currentWallValue = new int[4];

    [SerializeField] private int[] baseVampValue = new int[4];
    private int[] currentVampValue = new int[4];

    public int[] CurrentVampValue()
    {
        return currentVampValue;
    }

    private int[] maxStartMultiplier = new int[4];
    private int[] wallStartMultiplier = new int[4];
    private int[] vampStartMultiplier = new int[4];

    private int[] maxShieldedMultiplier = new int[4];
    private int[] wallShieldedMultiplier = new int[4];
    private int[] vampShieldedMultiplier = new int[4];

    [SerializeField] private int[] maxFieldedMultiplier = new int[4];
    private int[] wallFieldedMultiplier = new int[4];
    private int[] vampFieldedMultiplier = new int[4];

    [SerializeField] private Image[] statGUI = new Image[3];
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Image> images = new List<Image>();
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        circleCollider = this.gameObject.GetComponent<CircleCollider2D>();
        InitMultipliersTo1();
    }

    private void Start()
    {
        ApplyStartMultipliers();

        SetCurrentToMax();

        SetStatGUI();
    }

    public void SetCurrentToMax()
    {
        int numOfStats = 4;
        for (int i = 0; i < numOfStats; i++)
        {
            if (i < 2)
            {
                currentValue[i] = currentMaxValue[i];
            }
            else
            {
                currentValue[i] = currentMaxValue[i] / 2;
            }
        }
    }

    // Methods to adjust stats for having a shield or a field up
    public void ApplyStartMultipliers()
    {
        for (int i = 0; i < currentValue.Length; i++)
        {
            currentMaxValue[i] = baseMaxValue[i] * maxStartMultiplier[i];
            currentWallValue[i] = baseWallValue[i] * wallStartMultiplier[i];
            currentVampValue[i] = baseVampValue[i] * vampStartMultiplier[i];
        }

        if (shielded)
        {
            ApplyShieldedMultipliers();
        }

        if (fielded)
        {
            ApplyFieldedMultipliers();
        }
    }

    public void ApplyShieldedMultipliers()
    {
        if (shielded)
        {
            for (int i = 0; i < currentValue.Length; i++)
            {
                currentMaxValue[i] = currentMaxValue[i] * maxShieldedMultiplier[i];
                currentWallValue[i] = currentWallValue[i] * wallShieldedMultiplier[i];
                currentVampValue[i] = currentVampValue[i] * vampShieldedMultiplier[i];
            }
        }
    }

    public void ApplyFieldedMultipliers()
    {
        if (fielded)
        {
            for (int i = 0; i < currentValue.Length; i++)
            {
                currentMaxValue[i] = currentMaxValue[i] * maxFieldedMultiplier[i];
                currentWallValue[i] = currentWallValue[i] * wallFieldedMultiplier[i];
                currentVampValue[i] = currentVampValue[i] * vampFieldedMultiplier[i];
            }
        }
    }

    // These two methods are for special abilities... could use more refinement to
    // make them more generally useful
    public void AdjustFieldMultipliers(float mult, List<int> multiplierType, List<int> statType)
    {
        if (shielded)
        {
            for (int i = 0; i < maxFieldedMultiplier.Length; i++)
            {
                if (statType.Contains(i))
                {
                    if (multiplierType.Contains(0))
                    {
                        maxFieldedMultiplier[i] = (int)((float)maxFieldedMultiplier[i] * mult);
                    }
                    if (multiplierType.Contains(1))
                    {
                        wallFieldedMultiplier[i] *= (int)((float)wallFieldedMultiplier[i] * mult);
                    }
                    if (multiplierType.Contains(2))
                        vampFieldedMultiplier[i] *= (int)((float)vampFieldedMultiplier[i] * mult);
                }
            }
        }
    }

    public void AdjustShieldMultipliers(float mult, List<int> multiplierType, List<int> statType)
    {
        if (fielded)
        {
            for (int i = 0; i < maxShieldedMultiplier.Length; i++)
            {
                if (statType.Contains(i))
                {
                    if (multiplierType.Contains(0))
                    {
                        maxShieldedMultiplier[i] = (int)((float)maxShieldedMultiplier[i] * mult);
                    }
                    if (multiplierType.Contains(1))
                    {
                        wallShieldedMultiplier[i] *= (int)((float)wallShieldedMultiplier[i] * mult);
                    }
                    if (multiplierType.Contains(2))
                        vampShieldedMultiplier[i] *= (int)((float)vampShieldedMultiplier[i] * mult);
                }
            }
        }
    }


    // Interaction Methods -> Could port to another class
    public void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject.tag);
        bool thisPlayerCollEnemyOrPlayer = false;
        if (this.gameObject.tag.Length >= 6)
        {
            thisPlayerCollEnemyOrPlayer = (this.gameObject.tag.Substring(0, 6) == "Player" && (collision.gameObject.tag == "Enemy" || collision.gameObject.tag.Substring(0, 6) == "Player"));
        }


        bool thisEnemyCollPlayer = this.gameObject.tag == "Enemy" && collision.gameObject.tag.Substring(0, 6) == "Player";

        if (thisPlayerCollEnemyOrPlayer || thisEnemyCollPlayer)
        {
            Stats colStats = collision.gameObject.GetComponent<Stats>();
            // Do stuff for colliding with enemy or player
            // Because damage
            DecrementCurrentValue(0, colStats.CurrentValue()[2]);
            // Because vamp
            for (int i = 0; i < currentValue.Length; i++)
            {
                DecrementCurrentValue(i, CurrentVampValue()[i]);
            }
        }

        if (collision.gameObject.tag.Length >= 4)
        {
            if (collision.gameObject.tag.Substring(collision.gameObject.tag.Length - 4, 4) == "Wall")
            {
                // Do stuff for colliding with wall
                int statAffected = collision.gameObject.GetComponent<WallManager>().WallID;
                DecrementCurrentValue(statAffected, -currentWallValue[statAffected]);
            }
        }
        if (collision.gameObject.tag.Substring(0, 4) == "Bomb" &&
            collision.gameObject.tag.Substring(4, collision.gameObject.tag.Length - 4) != this.gameObject.tag)
        {
            DecrementCurrentValue(1, collision.gameObject.GetComponent<BombManager>().damage);
        }
    }

    public void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag.Substring(0, 5) == "Field" && trigger.transform.parent != this.transform)
        {
            fields.Add(trigger.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag.Substring(0, 5) == "Field" && trigger.transform.parent != this.transform)
        {
            fields.Remove(trigger.transform);
        }
    }


    // The key function in this class -> allows us to change the value of our current stats
    public void DecrementCurrentValue(int targetedStat, int amount)
    {
        if (!shielded || amount < 0)
        {
            // If you have enough to lose, lose it. If not, go to zero
            bool nonNegativeResult = currentValue[targetedStat] >= amount;
            bool notGreaterMaxResult = (currentValue[targetedStat] - amount) <= currentMaxValue[targetedStat];
            if (nonNegativeResult && notGreaterMaxResult)
            {
                currentValue[targetedStat] -= (amount);
                BalanceDamageSwitch(targetedStat);
                // If you lose damage, gain switch, and vice versa
            }
            else if (!nonNegativeResult)
            {
                currentValue[targetedStat] = 0;
                BalanceDamageSwitch(targetedStat);
            }
            else if (!notGreaterMaxResult)
            {
                currentValue[targetedStat] = currentMaxValue[targetedStat];
                BalanceDamageSwitch(targetedStat);
            }

            // Die if health <= 0
            if (currentValue[0] <= 0)
            {
                spriteRenderer.enabled = false;
                foreach (Image i in images)
                {
                    i.enabled = false;
                }
                circleCollider.enabled = false;
            }
        }

        SetStatGUI();
        FloatNumText(targetedStat, amount);
    }


    // Private methods
    private void SetStatGUI()
    {
        for (int i = 0; i < statGUI.Length; i++)
        {
            statGUI[i].fillAmount = (float)currentValue[i] / currentMaxValue[i];
        }


    }

    private void FloatNumText(int targetedStat, int amount)
    {
        Transform floatingNum = (Transform)Instantiate(floatingText, new Vector3(0f, 5f, 0f), Quaternion.identity);
        floatingNum.transform.SetParent(this.transform.GetChild(0), worldPositionStays: false);
        floatingNum.GetComponent<TextFloat>().value = -amount;
        floatingNum.GetComponent<TextFloat>().type = targetedStat;

    }


    // When damage goes up, switch goes down, and vice versa.
    private void BalanceDamageSwitch(int targetedStat)
    {
        if (targetedStat == 2)
        {
            currentValue[3] = currentMaxValue[2] - currentValue[2];
        }
        if (targetedStat == 3)
        {
            currentValue[2] = currentMaxValue[3] - currentValue[3];
        }
    }

    private void InitMultipliersTo1()
    {
        int numOfStats = 4;
        for (int i = 0; i < numOfStats; i++)
        {
            maxStartMultiplier[i] = 1;
            wallStartMultiplier[i] = 1;
            vampStartMultiplier[i] = 1;

            maxShieldedMultiplier[i] = 1;
            wallShieldedMultiplier[i] = 1;
            vampShieldedMultiplier[i] = 1;

            maxFieldedMultiplier[i] = 1;
            wallFieldedMultiplier[i] = 1;
            vampFieldedMultiplier[i] = 1;
        }
    }
}





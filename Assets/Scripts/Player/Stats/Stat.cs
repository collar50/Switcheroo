using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    private int currentHealth;
    private int currentMana;
    private int currentDS;

    private int maxHealth;
    private int maxMana;
    private int maxDS;

    public int mHealthWall;
    public int mManaWall;
    public int mDamageWall;
    public int mSwitchWall;

    public int mHealthVamp = 1;
    public int mManaVamp = 1;
    public int mDamageVamp = 1;
    public int mSwitchVamp = 1;


    [HideInInspector] public int tab;

    [SerializeField] public Image mHealthDisplay;    
    [SerializeField] public Image mManaDisplay;
    [SerializeField] public Image mDSDisplay;
    //private Image mHealthDisplayReflection;

    private void Start()
    {
        //mHealthDisplayReflection = mHealthDisplay.transform.GetChild(0).GetComponent<Image>();

        mMaxHealth = 100;
        mMaxMana = 100;
        mMaxDS = 25;

        mCurrentHealth = mMaxHealth;
        mCurrentMana = mMaxMana;
        mCurrentDS = mMaxDS;

        mHealthWall = 1;
        mManaWall = 1;
        mDamageWall = 1;
        mSwitchWall = 1;

        mHealthVamp = 1;
        mManaVamp = 1;
        mDamageVamp = 1;
        mSwitchVamp = 1;

}

    // ACCESSORS AND MUTATORS


    public int mMaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public int mMaxMana { get { return maxMana; } set { maxMana = value; } }

    public int mMaxDS { get { return maxDS; } set { maxDS = value; } }

    public int mCurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
                if (this.gameObject.tag == "Player")
                {
                    // Detatch Camera
                    // @TODO Make Camera Controller
                }
                Destroy(this.gameObject);
                // Destroy player
            }
            else if (value >= mMaxHealth)
            {
                currentHealth = mMaxHealth;
            }
            else
            {
                currentHealth = value;
            }

            mHealthDisplay.fillAmount = (float)currentHealth / mMaxHealth;
            // mHealthDisplayReflection.fillAmount = (float) currentHealth / mMaxHealth;            
        }
    }

    public int mCurrentMana
    {
        get
        {
            return currentMana;
        }

        set
        {
            if (value >= 0 && value < mMaxMana)
            {
                currentMana = value;
            }
            else if (value >= mMaxMana)
            {
                currentMana = mMaxMana;
            }

            if (this.gameObject.tag == "Player")
            {
                mManaDisplay.fillAmount = (float)currentMana / mMaxMana;
            }
        }
    }

    public int mCurrentDS
    {
        get
        {
            return currentDS;
        }

        set
        {
            if (value <= 0)
            {
                currentDS = 0;
            }
            else if (value >= mMaxDS)
            {
                currentDS = mMaxDS;
            }
            else
            {
                currentDS = value;
            }

            mDSDisplay.fillAmount = (float)currentDS / mMaxDS;
        }        
    }

    // OTHER INTERFACE METHODS
    public bool isGreaterThanCurrent(int type, int comparator)
    {
        int lCurrentValue = getCurrentOfType(type);

        if (comparator > lCurrentValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isCurrentEqualMax(int type)
    {
        int lCurrentValue = getCurrentOfType(type);
        int lMaxValue = getMaxOfType(type);

        if (lCurrentValue == lMaxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // SUPPORT METHODS
    private int getCurrentOfType(int type)
    {
        int lCurrentValue = 0;

        switch (type)
        {
            case 0:
                lCurrentValue = mCurrentHealth;
                break;
            case 1:
                lCurrentValue = mCurrentMana;
                break;
            case 2:
                lCurrentValue = mCurrentDS;
                break;
        }

        return lCurrentValue;
    }

    private int getMaxOfType(int type)
    {
        int lMaxValue = 0;

        switch (type)
        {
            case 0:
                lMaxValue = mMaxHealth;
                break;
            case 1:
                lMaxValue = mMaxMana;
                break;
            case 2:
                lMaxValue = mMaxDS;
                break;
        }

        return lMaxValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Enemy")
        {
            Stat stats = collision.gameObject.GetComponent<Stat>();
            stats.mCurrentHealth -= mCurrentDS;

            mCurrentHealth += mHealthVamp;
            stats.mCurrentHealth -= mHealthVamp;

            mCurrentMana += mManaVamp;
            stats.mCurrentMana -= mManaVamp;

            mCurrentDS += mSwitchVamp;
            stats.mCurrentDS -= mSwitchVamp;

            mCurrentDS -= mDamageVamp;
            stats.mCurrentDS += mDamageVamp;
        }
    }

}

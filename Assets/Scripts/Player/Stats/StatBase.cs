using UnityEngine;

public abstract class StatBase : MonoBehaviour
{
    [SerializeField] private int mCurrentValue;
    [SerializeField] private int mMaxValue;

    // ACCESSORS AND MUTATORS
    public int getCurrentValue()
    {
        return mCurrentValue;
    }

    public virtual void setCurrentValue(int pNewValue)
    {
        if (pNewValue <= 0)
        {
            mCurrentValue = 0;
            // Trigger death if health
        }
        else if (pNewValue >= mMaxValue)
        {
            mCurrentValue = mMaxValue;
        }
        else
        {
            mCurrentValue = pNewValue;
        }
    }

    public int MaxValue
    {
        get { return mMaxValue; }

        set
        {
            if (value <= 1)
            {
                mMaxValue = 1;
            }
            else
            {
                mMaxValue = value;
            }
        }
    }

    // OTHER INTERFACE METHODS
    public bool isGreaterThanCurrent(int comparator)
    {
        if (comparator > getCurrentValue())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isCurrentEqualMax()
    {
        if (getCurrentValue() == MaxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : StatBase {

    // Use this for initialization

    private void Awake()
    {
        const int BASE_MAX = 100;
        MaxValue = BASE_MAX;
        setCurrentValue(MaxValue);
    }

    public override void setCurrentValue(int pNewValue)
    {
        base.setCurrentValue(pNewValue);

        if (pNewValue <= 0)
        {
            // Destroy player
        }
    }
	
}

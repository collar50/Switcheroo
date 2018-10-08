using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DamageSwitch : StatBase {

    // Use this for initialization
    void Awake()
    {
        const int BASE_MAX = 25;
        MaxValue = BASE_MAX / 2;
        setCurrentValue(MaxValue);
    }

}

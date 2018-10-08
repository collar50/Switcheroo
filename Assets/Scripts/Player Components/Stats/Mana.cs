using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Mana : StatBase {

    // Use this for initialization
    void Awake()
    {
        const int BASE_MAX = 100;
        MaxValue = BASE_MAX;
        setCurrentValue(MaxValue);
    }
}

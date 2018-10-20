using UnityEngine;

[DisallowMultipleComponent]
public class Mana : StatBase
{
    // Use this for initialization
    private void Awake()
    {
        const int BASE_MAX = 100;
        MaxValue = BASE_MAX;
        setCurrentValue(MaxValue);
    }
}

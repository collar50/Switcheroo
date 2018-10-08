using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Mana), typeof(DamageSwitch))]
public class PlayerCollision : MonoBehaviour {

    Health health;
    Mana mana;
    DamageSwitch damageSwitch;

    private void Start()
    {
        health = GetComponent<Health>();
        mana = GetComponent<Mana>();
        damageSwitch = GetComponent<DamageSwitch>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Long Wall":
                collideWall(collision);
                break;
            case "Short Wall":
                collideWall(collision);
                break;
            case "Enemy":
                break;
            case "Player":
                break;
            case "Bomb":
                break;
        }
    }

    private void collideWall(Collision2D collision)
    {
        WallManager wallManager = collision.gameObject.GetComponent<WallManager>();
        switch (wallManager.wallType)
        {
            case WallManager.WallType.Health:
                health.setCurrentValue(health.getCurrentValue() + 2);
                break;

        }

        wallManager.setTypeEmpty(new Color(.2f, .2f, .2f, 1f));
    }
}

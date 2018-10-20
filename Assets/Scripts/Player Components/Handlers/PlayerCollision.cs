using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stat))]
public class PlayerCollision : MonoBehaviour {

    Stat stats;

    private void Start()
    {
        stats = this.GetComponent<Stat>();
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
                stats.mCurrentHealth += 2;
                break;
            case WallManager.WallType.Mana:
                stats.mCurrentMana += 2;
                break;
            case WallManager.WallType.Switch:
                stats.mCurrentDS -= 2;
                break;
            case WallManager.WallType.Damage:
                stats.mCurrentDS += 2;
                break;
            default:
                break;
        }

        wallManager.setTypeEmpty(new Color(.2f, .2f, .2f, 1f));
    }
}

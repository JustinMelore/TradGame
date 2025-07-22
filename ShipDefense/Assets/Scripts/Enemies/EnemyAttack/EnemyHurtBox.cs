using UnityEngine;

/// <summary>
/// Melee hurtbox for melee-type enemies
/// </summary>
public class EnemyHurtbox : Hurtbox

{
    private Ship ship;
    private PlayerController player;
    private bool hitParryBox;
    private bool hitPlayer;
    private bool hitShip;
    private bool hitPlayerOnce;
    private bool hitShipOnce;

    private void Awake()
    {
        ship = FindFirstObjectByType<Ship>();
        player = FindFirstObjectByType<PlayerController>();
        hitShipOnce = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //PlayerController player = collision.GetComponentInParent<PlayerController>();
            //if (player != null)
            //{
                //player.DamagePlayer(damage);
                //Debug.Log("Enemy hit the player! with Damage" + damage);
                hitPlayer = true;
                Deactivate();
            //}
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            SeaMeleeEnemy seaMeleeEnemy = GetComponentInParent<SeaMeleeEnemy>();
            if (seaMeleeEnemy != null)
            {
                //ship.DamageShip(damage);
                //Debug.Log("Enemy hit the ship for " + damage + " damage");
                hitShip = true;
                //Deactivate();
            }
        } else if(collision.CompareTag("PlayerParryTag"))
        {
            hitParryBox = true;
            Deactivate();
        }
    }


    //private void LateUpdate()
    //{
    //    if(!hitParryBox)
    //    {
    //        if(hitPlayer && !hitPlayerOnce)
    //        {
    //            player.DamagePlayer(damage);
    //            hitPlayerOnce = true;
    //            Debug.Log("Enemy hit the player! with Damage" + damage);
    //        } else if(hitShip && !hitShipOnce)
    //        {
    //            SeaMeleeEnemy seaMeleeEnemy = GetComponentInParent<SeaMeleeEnemy>();
    //            if (seaMeleeEnemy != null)
    //            {
    //                ship.DamageShip(damage);
    //                hitShipOnce = true;
    //                Debug.Log("Enemy hit the ship for " + damage + " damage");
    //            }
    //        }
    //    }
    //    hitPlayer = false;
    //    hitShip = false;
    //    hitParryBox = false;
    //}

    private void OnDisable()
    {
        if (!hitParryBox)
        {
            if (hitPlayer && !hitPlayerOnce)
            {
                player.DamagePlayer(damage);
                hitPlayerOnce = true;
                Debug.Log("Enemy hit the player! with Damage" + damage);
            }
            else if (hitShip && !hitShipOnce)
            {
                SeaMeleeEnemy seaMeleeEnemy = GetComponentInParent<SeaMeleeEnemy>();
                if (seaMeleeEnemy != null)
                {
                    ship.DamageShip(damage);
                    hitShipOnce = true;
                    Debug.Log("Enemy hit the ship for " + damage + " damage");
                }
            }
        }
        hitPlayer = false;
        hitShip = false;
        hitParryBox = false;
        hitPlayerOnce = false;
        hitShipOnce = false;
    }
}
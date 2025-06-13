using UnityEngine;

public class SeaMeleeEnemy : Enemy
{
    [Header("Melee Settings")]
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private EnemyHurtbox hurtbox;
    [SerializeField] private float attackDuration;


}

using UnityEngine;

/// <summary>
/// Handles behavior for melee attack hurtboxes
/// </summary>
public abstract class Hurtbox : MonoBehaviour
{
    protected int damage;

    public virtual void Activate(int dmg)
    {
        damage = dmg;
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}

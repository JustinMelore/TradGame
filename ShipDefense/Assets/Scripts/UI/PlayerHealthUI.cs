using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Animator animator;

    public void SetHealth(float current, float max)
    {
        float newHealthFill = current / max;
        if (newHealthFill > healthBarFill.fillAmount) animator.SetTrigger("Healed");
        else if (newHealthFill < healthBarFill.fillAmount) animator.SetTrigger("Damaged");
            healthBarFill.fillAmount = newHealthFill;
    }
}

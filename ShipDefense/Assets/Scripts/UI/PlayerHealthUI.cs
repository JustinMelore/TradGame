using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    public void SetHealth(float current, float max)
    {
        healthBarFill.fillAmount = current / max;
    }
}

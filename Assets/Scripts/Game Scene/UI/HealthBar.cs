using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
    }

    private void OnEnable()
    {
        slider.value = 100;
    }

    public void SetHealth(float @value)
    {
        slider.value = @value;
    }
}

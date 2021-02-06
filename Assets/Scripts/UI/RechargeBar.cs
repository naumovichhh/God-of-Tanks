using UnityEngine;
using UnityEngine.UI;

public class RechargeBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = 1;
        slider.value = 0;
    }

    public void SetValue(float @value)
    {
        slider.value = @value;
    }
}

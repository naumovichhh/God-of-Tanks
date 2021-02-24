using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        text.SetText($"Game over. Your score is {GameManager.Instance.Score.ToString(@"m\:ss")}.\nEnter player name:");
    }
}

using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        string text = $"Your score is {GameManager.Instance.Score.ToString(@"m\:ss")}.\nEnter player name:";
        if (GameManager.Instance.Won)
            text = $"You won! {text}";
        else
            text = $"You lost. {text}";
        
        textMesh.SetText(text);
    }
}

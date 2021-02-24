using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoresButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Highscores");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class OkButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Menu");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class DescriptionButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Description");
    }
}

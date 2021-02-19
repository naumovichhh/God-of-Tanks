using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        //throw new System.NotImplementedException();
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }
}

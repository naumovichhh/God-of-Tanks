using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOverScene
{
    public class OkButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField input;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClick();
            }
        }

        public void OnClick()
        {
            HighscoreStorage
                .Append(new Tuple<string, TimeSpan>(input.text, GameManager.Instance.Score));
            SceneManager.LoadScene("Menu");
        }
    }
}

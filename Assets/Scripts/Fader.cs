using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    public void LoadScene()
    {
        if (GameManager.Instance.LevelNumber == 0)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene("Level " + GameManager.Instance.LevelNumber);
        }
    }
}

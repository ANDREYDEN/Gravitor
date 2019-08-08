using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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

    public void FadeInScene(bool reverse=false)
    {
        _animator.SetBool("sceneActive", !reverse);
    }
}

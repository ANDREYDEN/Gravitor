using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    
    public GameObject MainMenu;
    public GameObject Score;
    public GameObject Pause;

    public static UIController Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnFullLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnFullLoad;
    }

    private void OnFullLoad(Scene scene, LoadSceneMode mode)
    {
        switch (GameManager.Instance.State)
        {
            case 1:
                Score.SetActive(true);
                break;
            case 0:
                Score.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Updates the score label.
    /// </summary>
    /// <param name="scoreNumber">The new score</param>
    public void UpdateScore(int scoreNumber)
    {
        Text scoreText = Score.GetComponentInChildren<Text>();
        scoreText.text = scoreNumber.ToString();
    }
}

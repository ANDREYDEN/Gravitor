using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    
    public GameObject MainMenuObj;
    public GameObject SettingsObj;
    public GameObject FaderObj;
    public GameObject ScoreObj;
    public GameObject PauseObj;

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

    private void Start()
    {
        FaderObj.SetActive(true);
        FaderObj.transform.SetSiblingIndex(999);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnFullLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnFullLoad;
    }

    /// <summary>
    /// Executes when the scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnFullLoad(Scene scene, LoadSceneMode mode)
    {
        switch (GameManager.Instance.State)
        {
            case 1:
                ScoreObj.SetActive(true);
                MainMenuObj.SetActive(false);
                SettingsObj.SetActive(false);
                break;
            case 0:
                MainMenuObj.SetActive(true);
                SettingsObj.SetActive(true);
                ScoreObj.SetActive(false);
                PauseObj.SetActive(false);
                MainMenuObj.GetComponent<MainMenu>().SetPhysics(false);
                //MainMenuObj.GetComponent<MainMenu>().ResetPosition();
                CameraControl.Instance.ResetPosition();
                break;
        }   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseObj.GetComponent<Pause>().PauseResume(!PauseObj.activeSelf);
        }
    }

    public void FadeInScene(bool reverse=false)
    {
        FaderObj.GetComponent<Fader>().FadeInScene(reverse);
    }

    /// <summary>
    /// Updates the score label.
    /// </summary>
    /// <param name="scoreNumber">The new score</param>
    public void UpdateScore(int scoreNumber)
    {
        Text scoreText = ScoreObj.GetComponentInChildren<Text>();
        scoreText.text = scoreNumber.ToString();
    }
}

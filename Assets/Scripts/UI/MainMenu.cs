using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public float delayBeforeStart;
    private bool _playClicked = false;
    
    public GameObject[] _physicalObjects;

    private void Start()
    {

        SetPhysics(false);
    }

    /// <summary>
    /// Turns the physics of the UI on or off
    /// </summary>
    /// /// <param name="on">on/off</param>
    public void SetPhysics(bool on)
    {
        for (int i = 0; i < _physicalObjects.Length; i++)
        {
            Rigidbody2D rb = _physicalObjects[i].GetComponent<Rigidbody2D>();
            rb.simulated = on;
            rb.gravityScale = 1 * ((i % 2 == 0) ? 1 : -1);
        }
    }

    /// <summary>
    /// For loading the game with a delay. See OnPlayClick()
    /// </summary>
    void StartGame()
    {
        GameManager.Instance.Respawn(PlayerPrefs.GetInt("currentLevel"));          // load the last level attempted
    }

    /// <summary>
    /// Executes when the 'Play' button on the main menu was clicked
    /// </summary>
    public void OnPlayClick()
    {
        if (!_playClicked)
        {
            _playClicked = true;
            Invoke("StartGame", delayBeforeStart);
        }
    }

    /// <summary>
    /// Closes the game
    /// </summary>
    public void Exit()
    {
        // TODO: save progress
        Application.Quit();
    }

    public void OnDisable()
    {
        Debug.Log("here");
    }
}

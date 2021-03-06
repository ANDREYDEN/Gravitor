﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float delayBeforeStart;

    private bool _playClicked = false;
    public GameObject[] _physicalObjects;
    private List<Vector3> _positions;

    private void Awake()
    {
        _positions = new List<Vector3>();
        SavePosition();
    }

    private void Start()
    {
        SetPhysics(false);
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
        _playClicked = false;
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
    /// Save position of all physical UI objects
    /// </summary>
    private void SavePosition()
    {
        for (int i = 0; i < _physicalObjects.Length; i++)
        {
            _positions.Add(_physicalObjects[i].transform.position);
        }
    }

    /// <summary>
    /// Restore the position of all physical UI objects
    /// </summary>
    public void ResetPosition()
    {
        Vector3 cameraPosition = CameraControl.Instance.transform.position;
        for (int i = 0; i < _physicalObjects.Length; i++)
        {
            _physicalObjects[i].transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0) + _positions[i];
            _physicalObjects[i].transform.rotation = Quaternion.identity;
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
        PlayerPrefs.Save();
        Application.Quit();
    }
}

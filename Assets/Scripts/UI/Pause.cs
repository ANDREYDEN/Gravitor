using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    /// <summary>
    /// Pauses/Resumes the game
    /// </summary>
    /// <param name="isPaused">Pause (true) or resume (false)</param>
    public void PauseResume(bool isPaused = true)
    {
        Time.timeScale = 1 - (isPaused ? 1 : 0);    // freeze time
        gameObject.SetActive(isPaused);              // show pause screen
    }

    public void OnPauseExit()
    {
        GameManager.Instance.Respawn(0);
    }
}

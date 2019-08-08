using UnityEngine;

public class Pause : MonoBehaviour
{
    /// <summary>
    /// Pauses/Resumes the game
    /// </summary>
    /// <param name="isPaused">Pause (true) or resume (false)</param>
    public void PauseResume(bool isPaused = true)
    {
        Player.Instance.FreezeToggle();                    // freeze player
        gameObject.SetActive(isPaused);                             // show/hide pause screen
    }

    public void OnPauseExit()
    {
        GameManager.Instance.Respawn(0);
    }
}

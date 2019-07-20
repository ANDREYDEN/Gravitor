using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject player;
    private Player playerScript;

    private void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    private void Update()
    {
        Text scoreText = GameObject.Find("ScoreNumber").GetComponent<Text>();
        scoreText.text = playerScript.crystals.ToString();
    }

    public void OnWinRestart()
    {
        playerScript.Respawn();
        ActivateWinBanner(false);
    }

    public static void ActivateWinBanner(bool activate)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject winBanner = canvas.transform.Find("WinBanner").gameObject;
        winBanner.SetActive(activate);
    }
}

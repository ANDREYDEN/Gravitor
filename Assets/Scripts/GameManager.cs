using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private int _levelNumber = 0;
    private int _crystals = 0;
    private readonly int _numberOfLevels = 3;
    private int _maxCrystals = 0;
    private int state = 0;

    public static GameManager Instance { get => instance; }
    public int State { get => state; }
    public int LevelNumber { get => _levelNumber; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", 1)); // if this is the first launch, set the currentLevel to 1
            _levelNumber = PlayerPrefs.GetInt("currentLevel");
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
        state = (scene.name == "Main Menu") ? 0 : 1;
        _crystals = 0;
        UIController.Instance.FadeInScene();
        GameObject pickables = GameObject.Find("Pickables");
        if (pickables)
        {
            Tilemap pickablesTM = pickables.GetComponent<Tilemap>();
            _maxCrystals = GetNumberOfTiles(pickablesTM);
        }
        UIController.Instance.UpdateScore(_crystals, _maxCrystals);
    }

    /// <summary>
    /// Reloads the scene after player's death/win
    /// </summary>
    /// <param name="levelNumber">The level number to respawn on</param>
    /// <param name="sameLevel">If true, disregards the 'level' parameter and respawns on the same level</param>
    public void Respawn(int levelNumber, bool sameLevel=false)
    {
        if (!sameLevel)
        {
            _levelNumber = levelNumber;
            if (_levelNumber > _numberOfLevels)
            {
                PlayerPrefs.SetInt("currentLevel", 1);
                _levelNumber = 0;                                       // go to main menu after completing all levels
            }
            else if (_levelNumber != 0)
            {
                PlayerPrefs.SetInt("currentLevel", _levelNumber);
            }
        }
        UIController.Instance.FadeInScene(true);                        // launching the fade out animation will load the next scene
    }
  
    /// <summary>
    /// Disables the player and shows the winning banner
    /// </summary>
    public void LevelComplete()
    {
        Respawn(_levelNumber + 1);
    }

    /// <summary>
    /// Increases the number of crystals by 1. Updates the UI.
    /// </summary>
    public void IncreaseScore()
    {
        UIController.Instance.UpdateScore(++_crystals, _maxCrystals);
    }

    /// <summary>
    /// O(n) way of calculating number of tiles
    /// </summary>
    /// <param name="tilemap">The tilemap </param>
    /// <returns></returns>
    public static int GetNumberOfTiles(Tilemap tilemap)
    {
        tilemap.CompressBounds();
        TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
        return tiles.Where(x => x != null).ToArray().Length;
    }
}

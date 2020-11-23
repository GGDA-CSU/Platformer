using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum State { PAUSED, PLAYING, GAMEOVER, MENU }

public class GameController : MonoBehaviour
{
    // World Values
    [SerializeField] private GameObject PauseMenu = null;
    [SerializeField] private GameObject DeathScreen = null;
    [SerializeField] private GameObject World = null;
    [SerializeField] private GameObject Spawn = null;
    [SerializeField] private Player player = null;

    // Game Controller Instance
    private static GameController _instance = null;
    public static GameController Instance { get { return _instance; } }

    // Game Data and Scene Count
    public static GameData currentData;
    public State gameState;

    private List<Scene> scenes;

    void Awake()
    {
        if (_instance == null)
        {
            gameState = State.PLAYING;

            scenes = new List<Scene>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                scenes.Add(SceneManager.GetSceneByBuildIndex(i));
            }

            currentData = new GameData();

            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //var Spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            //foreach (GameObject spawn in Spawns)
            //{
            //    SpawnPoints.Add(spawn);
            //}

            //World = GameObject.FindWithTag("World");
            //Spawn = GameObject.FindWithTag("Respawn");
            player = FindObjectOfType<Player>();

            //if (SceneManager.GetActiveScene().buildIndex > 1)
            //{
            //    ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));
            //}

            DontDestroyOnLoad(gameObject);
        }
        catch
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            // Game State Handling
            switch (gameState)
            {
                case State.MENU:
                    break;

                case State.PAUSED:
                    if (Input.GetButtonDown("Cancel"))
                    {
                        PlayGame();
                    }
                    break;

                case State.PLAYING:
                    // Player presses start or escape
                    if (Input.GetButtonDown("Cancel"))
                    {
                        PauseGame();
                    }

                    // If player is dead
                    if (player.tag == "Dead")
                    {
                        gameState = State.GAMEOVER;
                    }
                    break;

                case State.GAMEOVER:
                    // Use some type of System to see when player dies
                    GameOver();
                    break;
            }
        }
    }
    
    // Fix ----------------------------------------------------------------------
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        gameState = State.PAUSED;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        gameState = State.PLAYING;
    }

    public void GameOver()
    {
        // Have the audio switch to the death music, when player dies

        Time.timeScale = 0;
        DeathScreen.SetActive(true);
        World.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    // End Fix -----------------------------------------------------------------------

    [Serializable]
    public class GameData
    {
        public int points;

        public GameData()
        {
            points = 0;
        }
    }
}

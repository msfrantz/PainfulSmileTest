using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    private static GameManagerScript _instance;

    public GameObject timeUI, scoreUI, spawnerPrefab, gameOverScore, gameOverMenu;

    public float gameTime, spawnTime;

    private float playAgainTime, playAgainSpawnTime;

    private GameObject optionsMenu;


    int playerScore;



    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _instance.optionsMenu = GameObject.Find("OptionsMenu");
       
    }

    private void Start()
    {
       
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
        {
            Time.timeScale = 1.0f;

            _instance.timeUI = GameObject.Find("TimeText");
            _instance.scoreUI = GameObject.Find("ScoreText");
            _instance.gameOverScore = GameObject.Find("GameOverScoreText");
            _instance.gameOverMenu = GameObject.Find("GameOverMenu");

            _instance.gameOverMenu.SetActive(false);

            StartCoroutine("CountDown");
        }
        else
        {
            _instance.optionsMenu = GameObject.Find("OptionsMenu");
            
        }
    }
  
    IEnumerator CountDown()
    {
        while (true)
        {
            gameTime--;
            if (timeUI != null)
                _instance.timeUI.GetComponent<TextMeshProUGUI>().SetText("Time: " + gameTime.ToString());

            if (gameTime < 1)
            {
                GameOver();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void GameOver()
    {
        _instance.gameOverMenu.SetActive(true);
        _instance.gameOverScore.GetComponent<TextMeshProUGUI>().SetText("Score: " + playerScore.ToString());
        _instance.playerScore = 0;
        Time.timeScale = 0f;
        StopCoroutine("CountDown");
    }
    public void UpdateScore(int newScore)
    {
        _instance.playerScore += newScore;
        _instance.scoreUI.GetComponent<TextMeshProUGUI>().SetText("Score: " + playerScore.ToString());
    }

    public void OnClickedPlay()
    {
        _instance.playAgainTime = _instance.gameTime;
        _instance.playAgainSpawnTime = _instance.spawnTime;

        spawnerPrefab.GetComponent<Spawner>().spawnTime = _instance.spawnTime;
        DontDestroyOnLoad(_instance.gameObject);

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        
    }

    public void OnClickedOptions()
    {
        if (_instance.optionsMenu.activeInHierarchy)
        {
            _instance.optionsMenu.SetActive(false);
        }
        else
        {
            _instance.optionsMenu.SetActive(true);
        }
    }

    public void OnClickedQuit()
    {
        _instance.gameTime = 60f;
        _instance.spawnTime = 40f;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

    }
    public void OnClickedPlayAgain()
    {
        _instance.gameTime = _instance.playAgainTime;
        _instance.spawnTime = _instance.playAgainSpawnTime;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

    }
}

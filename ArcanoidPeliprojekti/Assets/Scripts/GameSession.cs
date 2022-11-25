using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    

    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] int startLives;

    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameWinText;
    [SerializeField] GameObject restartButton;
    public GameObject pausePanel;

    
    int currentLives;
    int currentScore;
    int currentHighScore;
    bool uiCheck=false;


    
    private void Awake()
    {

        int instanceCount = FindObjectsOfType<GameSession>().Length;
        if (instanceCount > 1)
        {
            Destroy(gameObject);
           
        }
        else
        {
            DontDestroyOnLoad(gameObject);
           
        }
       
    }
    void Start()
    {
        

        currentLives = startLives;

        restartButton.GetComponent<Button>().onClick.AddListener(Reset);

        currentHighScore = PlayerPrefs.GetInt("highscore");
        
    }

    // Update is called once per frame
    void Update()
    {
        CountBalls();

        CountBlocks();

        UpdateUI();

        if (uiCheck)
        {
            ResetScreen();
            uiCheck = false;
        }
        if (Time.timeScale == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            pauseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            resumeGame();
            pausePanel.SetActive(false);
        }
    

    }
    private void resumeGame()
    {
        UnPause();
    }
    private void pauseMenu()
    {
        Pause();
    }

    private void CountBalls()
    {
        var Balls = GameObject.FindGameObjectsWithTag("Ball");
        if (Balls.Length == 0)
        {
            DecreaseLives();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResetBall();

        }
    }

    private void CountBlocks()
    {
        var blocks = GameObject.FindGameObjectsWithTag("Block");

        if (blocks.Length == 0)
        {
            //Do winning stuff

            win();
        }
    }

    private void UpdateUI()
    {
        livesText.text = currentLives.ToString();
        scoreText.text = currentScore.ToString();
        highScoreText.text = currentHighScore.ToString();
    }

    public void increaseScore(int value)
    {
        currentScore += value;
    }
    public void DecreaseLives()
    {
        currentLives--;
        if(currentLives <= 0)
        {
            GameOver();
        }
    }

    private void win()
    {
        var numberOfScenes = SceneManager.sceneCountInBuildSettings;
        var currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if(currentBuildIndex == numberOfScenes -1)
        {
            //Declare winner
            blackScreen.SetActive(true);
            gameWinText.SetActive(true);
            restartButton.SetActive(true);
            
            SetHighScore();
            Pause();
        }
        else
        {
            //Load Next Scene
            SceneManager.LoadScene(currentBuildIndex + 1);
        }
    }

    private void GameOver()
    {
        blackScreen.SetActive(true);
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        SetHighScore();
        Pause();
    }

    
    private void Reset()
    {
      
        ResetScreen();
        ResetScore();
        ResetLevel1();
        uiCheck = true;
    }
    private void ResetScreen()
    {

        UnPause();
        blackScreen.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        gameWinText.SetActive(false);
        
        
    }

    private void ResetScore()
    {
        currentLives = startLives;
        currentScore = 0;
    }
    private void ResetLevel1()
    {
        SceneManager.LoadScene(0);
    }
    private void SetHighScore()
    {
        if (currentScore > currentHighScore)
        {
            currentHighScore = currentScore;
            PlayerPrefs.SetInt("highscore",currentHighScore);
        }
    }
    private void Pause()
    {
        Time.timeScale = 0f;
    }
    private void UnPause()
    {
        Time.timeScale = 1f;
        
        
    }

   
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameSession : MonoBehaviour
{

    public Animator transition;

    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] int startLives;

    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameWinText;
    [SerializeField] GameObject restartButton;
 
    public GameObject pausePanel;

    public float transitionTime=1f;



   
    int currentLives;
    int currentScore;
    int currentHighScore;
    bool uiCheck=false;

    public static GameSession instance = null;
    
   
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        UnPause();
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
        PauseMenu();

    }

    private void PauseMenu()
    {
        if (Time.timeScale == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnPause();
            pausePanel.SetActive(false);
        }
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
            Pause();
            StartCoroutine(levelChange(transitionTime));
        }
    }
    IEnumerator levelChange(float transitionTime)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
[System.Serializable]
class PlayerData
{
    public int health;
    public int score;
    public int level;
    public float[] playerPos = new float[3];
    public float[] ballPos = new float[3];

}
[System.Serializable]
class BlockData
{
    public string blockName;
    public float[] blockPos =new float[3];
    public int blockHealth;
    public int blockScoreValue;
    
}

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
    [SerializeField] GameObject restartButton;

    int currentLives;
    int currentScore;
    int currentHighScore;
    // Start is called before the first frame update

    private void Awake()
    {
        int instanceCount = FindObjectsOfType<GameSession>().Length;
        if(instanceCount > 1)
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
        var Balls = GameObject.FindGameObjectsWithTag("Ball");
        if (Balls.Length == 0)
        {
            DecreaseLives();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResetBall();

        
        }

        var blocks = GameObject.FindGameObjectsWithTag("Block");


        if (blocks.Length == 0)
        {
            //Do winning stuff

            win();

        }
        
        
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
            Time.timeScale = 0f;
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
        Time.timeScale = 0f;
    }

    
    private void Reset()
    {
        ResetScore();
        ResetScreen();
        ResetLevel1();
    }
    private void ResetScreen()
    {
        blackScreen.SetActive(false);
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ResetScore()
    {
        currentLives = startLives;
        currentScore = 0;
    }
    private void ResetLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

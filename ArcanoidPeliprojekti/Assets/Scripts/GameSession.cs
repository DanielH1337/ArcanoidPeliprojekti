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
    [SerializeField] int powerUpTime;
    [SerializeField] GameObject paddle;
    
    
    int time = 0;
    int currentLives;
    int currentScore;
    int currentHighScore;
    bool uiCheck=false;

    private Vector3 scaleChange;
    
    [SerializeField] AudioSource powerupSound;

    public int growSize;
    public float shrinkSize;
    



    
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
        
       
        if (powerupSound != null)
        {
            powerupSound.GetComponent<AudioSource>();
        }
        
        if (paddle != null)
        {
            paddle.GetComponent<Transform>();
        }
            

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
            powerupSound.Stop();
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
        powerupSound.Stop();
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
    public void increasePaddleLenght()
    {
        if (time == powerUpTime)
        {
            powerupSound.Play();
            Debug.Log("Testi");
            time = 0;
            StartCoroutine(slowDecrease());
            
        }
        else
        {
            powerupSound.Play();
            StartCoroutine(slowIncrease());
            StartCoroutine(PowerUpTimer());
        }

    }
    private IEnumerator PowerUpTimer()
    {
        while(time < powerUpTime)
        {
            time += 1;
            Debug.Log(time);
            yield return new WaitForSeconds(1);
        }
        print("moi");
        increasePaddleLenght();
        
    }
    private IEnumerator slowIncrease()
    {
        scaleChange = new Vector3(1f, 0.0f, 0.0f);

        while (true)
        {
            if(growSize > paddle.transform.localScale.x)
            {
                paddle.transform.localScale += scaleChange;
            }
            if(growSize == paddle.transform.localScale.x)
            {
                powerupSound.Stop();
            }
            yield return new WaitForSeconds(0.0001f);
        }
       
        
    }
    private IEnumerator slowDecrease()
    {

        scaleChange = new Vector3(-1f, 0.0f, 0.0f);

        while (shrinkSize+1 < paddle.transform.localScale.x)
        {
          
            paddle.transform.localScale += scaleChange;
            Debug.Log(paddle.transform.localScale.x);
            if(paddle.transform.localScale.x == shrinkSize+1)
            {
                scaleChange = new Vector3(0.0f, 0.0f, 0.0f);
                powerupSound.Stop();
            }
          
            yield return new WaitForSeconds(0.0001f);

        }
           
        



    }
   
}

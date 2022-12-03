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

    public GameObject player;
    public GameObject ball;

    public GameObject[] blocks;
 
    public GameObject pausePanel;

    public float transitionTime=1f;
    private int levelIndex;
    private int currentLives;
    private int currentScore;
    private int currentHighScore;
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
            ball = GameObject.FindGameObjectWithTag("Ball");

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
    public void ResetLevel1()
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
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        data.health = currentLives;
        data.score = currentScore;
        data.level = SceneManager.GetActiveScene().buildIndex;

        data.ballvelocity[0] = ball.GetComponent<Rigidbody2D>().velocity.x;
        data.ballvelocity[1] = ball.GetComponent<Rigidbody2D>().velocity.y;

        data.playerPos[0] = player.transform.position.x;
        data.playerPos[1] = player.transform.position.y;
        data.playerPos[2] = player.transform.position.z;

        data.ballPos[0] = ball.transform.position.x;
        data.ballPos[1] = ball.transform.position.y;
        data.ballPos[2] = ball.transform.position.y;
        bf.Serialize(file, data);
        file.Close();

    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            levelIndex = data.level;
            currentLives = data.health;
            currentScore = data.score;
            Vector3 playerPosition;
            playerPosition.x = data.playerPos[0];
            playerPosition.y = data.playerPos[1];
            playerPosition.z = data.playerPos[2];
            player.transform.position = playerPosition;
            Vector3 ballPosition;
            ballPosition.x = data.ballPos[0];
            ballPosition.y = data.ballPos[1];
            ballPosition.z = data.ballPos[2];
            ball.transform.position = ballPosition;
            Vector2 ballvelocity;
            ballvelocity.x = data.ballvelocity[0];
            ballvelocity.y = data.ballvelocity[1];
            ball.GetComponent<Rigidbody2D>().velocity = ballvelocity;

        }
    }
    
}
[Serializable]
class PlayerData
{
    public int health;
    public int score;
    public int level;
    public float[] ballvelocity=new float[2];
    public float[] playerPos = new float[3];
    public float[] ballPos = new float[3];

}



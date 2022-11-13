using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameSession : MonoBehaviour
{

    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] int startLives;

    [SerializeField] Image blackScreen;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] Button restartButton;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        var Balls = GameObject.FindGameObjectsWithTag("Ball");
        if (Balls.Length <= 0)
        {
            DecreaseLives();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ResetBall();

        
        }
        
        
        livesText.text = currentLives.ToString();
        scoreText.text = currentScore.ToString();


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

    private void GameOver()
    {
        throw new NotImplementedException();
    }
}

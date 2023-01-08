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

    public  List<Vector3> blockPosList=new List<Vector3>();
    public static List<int> healthlist=new List<int>();

    public List<block> blockPrefabs; 
    public static List<block> blocks = new List<block>();

    public Animator transition;
    const string block_sub = "/block";
    const string block_Count_sub = "/block.count";
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] int startLives;

    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameWinText;
    [SerializeField] GameObject restartButton;


    public Button SaveButton;
    public GameObject player;
    public GameObject ball;

 
    public GameObject pausePanel;

    public float transitionTime=1f;
    private int currentLives;
    private int currentScore;
    private int currentHighScore;
    private float ballposY;
    bool uiCheck=false;

    public bool loadBool = false;

   // public static GameSession instance;
    private void Awake()
    {
        foreach (block block in blocks)
        {
            Debug.Log(block);

        }
    }
    void Start()
    {
        
        Debug.Log(loadBool);
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            currentScore = PlayerPrefs.GetInt("score");
            startLives = PlayerPrefs.GetInt("health");

        }
        loadBool = (PlayerPrefs.GetInt("loadBool")!=0);
        Debug.Log(loadBool);
        
        UnPause();
        currentLives = startLives;
        
        restartButton.GetComponent<Button>().onClick.AddListener(Reset);
      
        currentHighScore = PlayerPrefs.GetInt("highscore");
        ballposY = ball.transform.position.y;
        

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

        if (SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("Scene")&&loadBool == true)
        {
           // LoadBlocks();
            Debug.Log("testi1");
            block.loadblocks = true;
            loadBool = false;
            Load();
            PlayerPrefs.SetInt("loadBool", (loadBool ? 1 : 0));
        }

    }
    public void setSaveButtonInactive()
    {
        SaveButton.interactable = false;
        pausePanel.SetActive(false);
        UnPause();
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
        PlayerPrefs.SetInt("score", currentScore);
        PlayerPrefs.SetInt("health", currentLives);

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
    public void SaveBlocks()
    {
       
       /* string[] filePaths = Directory.GetFiles(Application.persistentDataPath); 
        foreach (string filePath in filePaths)
        {
            File.Delete(filePath);
        }*/
        
      
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + block_sub + SceneManager.GetActiveScene().buildIndex;
        string countpath = Application.persistentDataPath + block_Count_sub + SceneManager.GetActiveScene().buildIndex;

        FileStream countStream = new FileStream(countpath, FileMode.Create);
        formatter.Serialize(countStream, blocks.Count);
        countStream.Close();


        for (int i=0;i < blocks.Count; i++)
        {
           
            if (blocks[i] != null)
            {
                FileStream stream = new FileStream(path + i, FileMode.Create);
                BlockData bdata = new BlockData(blocks[i]);
                formatter.Serialize(stream, bdata);
                stream.Close();
            }
        
        }
    }
  
    public void LoadBlocks()
    {
        Debug.Log("Load Testi");
        blockPosList.Clear();
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + block_sub + SceneManager.GetActiveScene().buildIndex;
        string countpath = Application.persistentDataPath + block_Count_sub + SceneManager.GetActiveScene().buildIndex;
        int blockCount = 0;
        if (File.Exists(countpath))
        {
            
            FileStream countStream = new FileStream(countpath, FileMode.Open);
            blockCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }
        else
        {
            Debug.LogError("file not found in "+countpath);
        }

        for (int i = 0; i < blockCount; i++)
        {
            if (File.Exists(path + i))
            {
                
                FileStream stream = new FileStream(path + i, FileMode.Open);
                BlockData bdata = formatter.Deserialize(stream) as BlockData;
                stream.Close();
                Vector3 position = new Vector3(bdata.blockPos[0], bdata.blockPos[1], bdata.blockPos[2]);
               // Debug.Log(position);
                blockPosList.Add(position);
                healthlist.Add(bdata.blockHealth);
                
            }
            else
            {
                Debug.LogError("file not found in " + path+i);
            }
            
        }
        File.Delete(path);
       
    }
    public void DeleteFile(string filename)
    {
        string path = Application.persistentDataPath + "/" + filename;
        File.Delete(path);
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
        PlayerPrefs.SetInt("Scene",SceneManager.GetActiveScene().buildIndex);

    }
  
    public void Load()
    {
        if(SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("Scene"))
        {
            loadBool = true;
            PlayerPrefs.SetInt("loadBool", (loadBool ? 1 : 0));
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
            Debug.Log("loadkutsuttu");
        }
        else if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            LoadBlocks();
            var blockPosListlen = blockPosList.Count;
            int failCount;
            int blockPrefabsCount = blockPrefabs.Count;
            for(int x=0; x < blockPrefabsCount; x++)
            {
                failCount = 0;
                Vector3 blockvector3 = blockPrefabs[x].transform.position;
                for (int i = 0; i < blockPosListlen; i++)
                {
                    if(blockvector3 != blockPosList[i])
                    {
                        failCount += 1;
                        if (failCount == blockPosListlen)
                        {
                         //   blockPrefabs.Remove(blockPrefabs[x]);
                            blockPrefabs[x].SetActiveFalse();
                        }
                    }
                  
                }
            }
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
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
            if (ball.transform.position.y != ballposY)
            {
                FindObjectOfType<ball>().Launch(ballvelocity);
            }
            
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
[Serializable]
class BlockData
{
    public int blockHealth;
    public string blockName;
    public float[] blockPos;

    public BlockData(block block)
    {
        blockHealth = block.health;
        blockName = block.name;
        Vector3 blockPosition = block.transform.position;
        blockPos = new float[]
        {
            blockPosition.x, blockPosition.y, blockPosition.z
        };
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    public GameObject loadButton;
    public GameObject exitButton;
    bool loadControl;
    // Start is called before the first frame update
    void Start()
    {
         
        startButton.GetComponent<Button>().onClick.AddListener(Startgame);
        loadButton.GetComponent<Button>().onClick.AddListener(LoadGame);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Startgame()
    {
      //  File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        loadControl = (PlayerPrefs.GetInt("loadBool") != 0);
        loadControl = true;
        PlayerPrefs.SetInt("loadBool", (loadControl ? 1 : 0));
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
        
    }
    public void ExitGame()
    {
        Application.Quit();

    }
   
}

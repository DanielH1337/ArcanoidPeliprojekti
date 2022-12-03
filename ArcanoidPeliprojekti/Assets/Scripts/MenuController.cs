using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    public GameObject loadButton;
    public GameObject exitButton;
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
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        Debug.Log("Loading");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

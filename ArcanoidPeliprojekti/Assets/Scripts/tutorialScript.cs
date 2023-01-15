using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class tutorialScript : MonoBehaviour
{
    private int counter = 0;
    public GameObject EscTutorial;
    public GameObject tutorialPanel;
    public GameObject wasdTutorial;
    public GameObject launchBallTutorial;
    public GameObject paddle;
    public AudioSource startSound;
 
    
    private int index =0;

    private bool buttonCheck =true;
    private bool escCheck =true;
    private bool launchCheck=true;
    // Start is called before the first frame update

  
    void Start()
    {
        buttonCheck =true;
        escCheck =true;
        launchCheck=true;
        if(File.Exists(Application.persistentDataPath + "/Info"))
        {
            tutorialPanel.SetActive(false);
            gameObject.GetComponent<tutorialScript>().enabled = false;
            
        }
        else
        {
            startSound.Play();
            paddle.GetComponent<PlayerController>().canJump = false;
            tutorialPanel.SetActive(true);
            EscTutorial.SetActive(true);
            wasdTutorial.SetActive(false);
            launchBallTutorial.SetActive(false);
            pauseGame();
        }
       
        
  
    }
    

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape)&&index ==0)
        {
            if (escCheck==true)
            {
                counter += 1;
                if (counter == 2)
                {
                    escCheck = false;
                    index += 1;
                    counter = 0;
                    wasdTutorialView();
                }
            }
            
        }
        if(Input.GetKeyUp(KeyCode.A)&&index == 1)
        {
            if (buttonCheck==true)
            {
                index += 1;
                buttonCheck = false;
              
            }
            
            
        }
        if(Input.GetKeyDown(KeyCode.D)&&index == 2)
        {
            if(launchCheck == true)
            {
                index += 1;
                if (index == 3)
                {
                    launchCheck = false;
                    StartCoroutine(launchBallDelayed());
                }
            }
            
        }
    }
    IEnumerator launchBallDelayed()
    {
        yield return new WaitForSeconds(1);
        launchBallTutorialView();
    }
    public void okPressed()
    {
        ResumeGame();
        tutorialPanel.SetActive(false);
    }
    public void wasdTutorialView()
    {
        pauseGame();
        EscTutorial.SetActive(false);
        tutorialPanel.SetActive(true);
        wasdTutorial.SetActive(true);
    }
    public void launchBallTutorialView()
    {
        pauseGame();
        paddle.GetComponent<PlayerController>().canJump = true;
        tutorialPanel.SetActive(true);
        wasdTutorial.SetActive(false);
        launchBallTutorial.SetActive(true);
    }
    public void pauseGame()
    {
        StartCoroutine(LatePause());
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    IEnumerator LatePause()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0;
    }
}

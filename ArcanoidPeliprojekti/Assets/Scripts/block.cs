using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class block : MonoBehaviour
{
    public ParticleSystem explosion;
    public int health = 1;
    int scoreValue = 10;
    private Color orginalColor;
    public static bool loadblocks=false;


    void Awake()
    {
        if(this != null)
        {
            GameSession.blocks.Add(this);
        }
        
    }
    private void Start()
    {   
        orginalColor = gameObject.GetComponent<Renderer>().material.color;
     


    }
    public void Update()
    {
        if (loadblocks == true)
        {
            blockCounter();
            loadblocks = false;
        }
    }
    public void blockCounter()
    {
       
       
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            StartCoroutine(ColorChanger());
            health--;
            if(health <= 0)
            {
                explosion.Play();
                FindObjectOfType<GameSession>().increaseScore(scoreValue);
                FindObjectOfType<GameSession>().blockPrefabs.Remove(this);
                GameSession.blocks.Remove(this);
                Destroy(gameObject);
            }
        }
    }
    IEnumerator ColorChanger()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = orginalColor;
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    
}

  

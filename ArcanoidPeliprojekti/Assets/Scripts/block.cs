using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class block : MonoBehaviour
{
    public ParticleSystem explosion;
    [SerializeField] int health = 1;
    [SerializeField] int scoreValue = 10;
    private Color orginalColor;

    private void Start()
    {   
        orginalColor = gameObject.GetComponent<Renderer>().material.color;
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
    
}

  

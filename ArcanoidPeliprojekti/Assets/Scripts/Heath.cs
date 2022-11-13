using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heath : MonoBehaviour
{

    [SerializeField] int health = 1;
    [SerializeField] int scoreValue = 10;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            health--;
            if(health <= 0)
            {
                FindObjectOfType<GameSession>().increaseScore(scoreValue);
                Destroy(gameObject);
            }
        }
    }
}

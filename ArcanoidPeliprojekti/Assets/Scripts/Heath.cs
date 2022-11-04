using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heath : MonoBehaviour
{

    [SerializeField] int health = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

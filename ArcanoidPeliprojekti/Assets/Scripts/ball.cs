using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] float speed = 15f;
    [SerializeField] float accelaration = 1.0f;
    public int numberOfBallPowerup=3;
 
    private float  tempSpeed = 0.0f;

    Rigidbody2D rb2D;
    AudioSource audiosource;
 

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        audiosource = GetComponent<AudioSource>();
      
    }
    private void Start()
    {
        tempSpeed = speed;
    }
    private void Update()
    {
        rb2D.velocity = rb2D.velocity.normalized * tempSpeed;
       
        
    }
    private IEnumerator IncreaseBallSpeed()
    {
        while (tempSpeed < 30)
        {
            tempSpeed += accelaration;
            yield return new WaitForSeconds(1);
        }
    }

    public void Launch(Vector2 direction)
    {
        tempSpeed = speed;
        transform.parent = null;
        rb2D.simulated = true;
        StartCoroutine(IncreaseBallSpeed());
        rb2D.velocity = direction.normalized * speed;
    }

    public void Catch(Transform parent)
    {
        transform.parent = parent;
        rb2D.simulated = false;
        rb2D.velocity = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var blocks = GameObject.FindGameObjectsWithTag("Block");
        if (audiosource != null)
        {
            audiosource.Play();
        }
        if (collision.gameObject.name == "powerUpScale")
        {
            if (blocks.Length > 1)
            {
                //FindObjectOfType<ScaleLerper>().StartFunction();
                GameObject.FindGameObjectWithTag("Player").GetComponent<ScaleLerper>().StartFunction();
            }
            
        }
        if(collision.gameObject.name == "powerUpBalls")
        {
            StartCoroutine(launchAuto());
        }
        IEnumerator launchAuto()
        {
            for (int x = 0; x < numberOfBallPowerup; x++)
            {
                FindObjectOfType<PlayerController>().ResetBall();
                FindObjectOfType<PlayerController>().AutoLaunch();
                yield return new WaitForSeconds(0.5f);
            }
        }
        if(collision.gameObject.name == "powerUpBallScale")
        {
            if(blocks.Length > 1)
            {
                gameObject.GetComponent<ScaleLerper>().StartFunction();
            }
           
        }
    }
}

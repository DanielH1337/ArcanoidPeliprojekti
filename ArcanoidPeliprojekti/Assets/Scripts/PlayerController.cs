using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] Vector2 launchdir = new Vector2(1, 4);
    [SerializeField] GameObject ballPrefab;

    Rigidbody2D rb2D;
    Vector3 balloffset;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ball Ball = GetComponentInChildren<ball>();
        balloffset = Ball.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.velocity = new Vector2(Input.GetAxis("Horizontal")*speed, 0);

        if(transform.childCount > 0 && Input.GetButtonDown("Jump"))
        {
            ball ball = GetComponentInChildren<ball>();
            ball.Launch(launchdir);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            ResetBall();
        }
    }

    public void ResetBall()
    {
        ball ball = Instantiate(ballPrefab).GetComponent<ball>();
        ball.transform.parent = transform;
        ball.transform.position = transform.position + balloffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] float speed = 15f;

    Rigidbody2D rb2D;
    AudioSource audiosource;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        audiosource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        rb2D.velocity = rb2D.velocity.normalized * speed;
    }

    public void Launch(Vector2 direction)
    {
        transform.parent = null;
        rb2D.simulated = true;

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
        audiosource.Play();
    }
}

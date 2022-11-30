using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBounce : MonoBehaviour
{

    BoxCollider2D bc2D;

    private void Awake()
    {
        bc2D = GetComponent<BoxCollider2D>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Find the relative point. (Ball.x - Paddle.x) / Paddle.widht
        if (collision.gameObject.tag == "Ball")
        {
            float relativePosition = GetRelativePosition(collision.transform);


            //Change the velocity of the ball depending on the relative point.
            collision.rigidbody.velocity = new Vector2(relativePosition, 1).normalized * collision.rigidbody.velocity.magnitude;
        }




    }

    public float GetRelativePosition(Transform collision)
    {
        return (collision.transform.position.x - transform.position.x) / bc2D.bounds.size.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float gravityConstans;
    [SerializeField] private int disintegrationMass;
    [SerializeField] private float disableColliderTime;

    public Rigidbody2D rigidbody;

    private Vector3 startScale;
    private float startMass;
    private CircleCollider2D collider;
    private bool isFull;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        startScale = transform.localScale;
        startMass = rigidbody.mass;
        StartCoroutine(Disintegration());
        StartCoroutine(ChangeGravity());
    }

    private void FixedUpdate()
    {
        foreach (Ball ball in GravityManager.Instance.balls)
        {
            if (ball != this)
            {
                Rigidbody2D rigidbodyToAttract = ball.rigidbody;

                Vector2 direction = rigidbody.position - rigidbodyToAttract.position;
                float distance = direction.magnitude;

                float forceMagnitude = -gravityConstans * (rigidbody.mass * rigidbodyToAttract.mass) / (distance * distance);
                Vector2 force = direction.normalized * forceMagnitude;

                rigidbodyToAttract.AddForce(force);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFull == false)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ball") && collision.gameObject.GetComponent<Rigidbody2D>().mass == rigidbody.mass && collision.gameObject.transform.position.y < this.transform.position.y)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();
                float attractedObjectMass = ball.GetComponent<Rigidbody2D>().mass;
                rigidbody.mass = rigidbody.mass + attractedObjectMass;
                BallObjectPool.Instance.ReturnBall(ball);
                transform.localScale = startScale * rigidbody.mass;
            }
            else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ball") && collision.gameObject.GetComponent<Rigidbody2D>().mass < rigidbody.mass)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();
                float attractedObjectMass = ball.GetComponent<Rigidbody2D>().mass;
                rigidbody.mass = rigidbody.mass + attractedObjectMass;
                BallObjectPool.Instance.ReturnBall(ball);
                transform.localScale = startScale * rigidbody.mass;
            }
        }
    }

    IEnumerator ChangeGravity()
    {
        while (true)
        {
            if (GravityManager.Instance.balls.Count == GravityManager.Instance.ballsAmount)
            {
                gravityConstans = Mathf.Abs(gravityConstans);
                isFull = true;
            }
            yield return null;
        }
    }

    IEnumerator Disintegration()
    {
        while (true)
        {
            if (rigidbody.mass >= disintegrationMass)
            {
                rigidbody.mass = startMass;
                transform.localScale = startScale;
                collider.enabled = false;
                yield return new WaitForSeconds(disableColliderTime);
                collider.enabled = true;
            }

            yield return null;
        }
    }
}


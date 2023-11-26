using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce = 3f;
    private Rigidbody2D rb;

    private Vector3 previousPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += horizontalInput * speed * Time.deltaTime;
        CameraController.Instance.FocusTo(pos - transform.position);
        transform.position = pos;

        //Vector2 movement = new Vector2(horizontalInput, 0f);
        //movement.Normalize();
        //rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        float jump = Input.GetAxis("Jump");
        Vector2 moment = new Vector2(0f, jump);
        rb.velocity = new Vector2(rb.velocity.x, moment.y * jumpForce + jumpForce * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce = 3f;
    private Rigidbody2D rb;

    private Vector3 previousPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        previousPos = transform.position;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        float xPos = transform.position.x;
        if (xPos < CameraController.Instance.Left || xPos > CameraController.Instance.Right)
        {
            return;
        }
        // 0.00001
        if (Mathf.Abs(xPos - previousPos.x) > 1e-5f)
        {
            CameraController.Instance.FocusTo(transform.position - previousPos);
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += horizontalInput * speed * Time.deltaTime;
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

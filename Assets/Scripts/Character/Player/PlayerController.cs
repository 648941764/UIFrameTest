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

    [SerializeField] Transform rayTrans;

    [SerializeField] float rayDistance = 0.5f;

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
        previousPos = transform.position;
        float horizontalInput = Input.GetAxis("Horizontal");

        Flip(horizontalInput > 0f);

        //RaycastHit2D hit = Physics2D.Raycast(rayTrans.position, horizontalInput > 0f ? Vector2.right : Vector2.left, rayDistance);
        //if (hit.collider != null)
        //{
        //    return;
        //}

        Vector3 pos = transform.position;
        pos.x += horizontalInput * speed * Time.deltaTime;
        transform.position = pos;
        //CameraController.Instance.FocusPlayer();
        CameraController.Instance.FocusPlayer();
        //if (Mathf.Abs(xPos - previousPos.x) > 1e-5f)
        //{
        //    CameraController.Instance.FocusTo(transform.position - previousPos);
        //}
    }

    private void Jump()
    {
        float jump = Input.GetAxis("Jump");
        Vector2 moment = new Vector2(0f, jump);
        rb.velocity = new Vector2(rb.velocity.x, moment.y * jumpForce + jumpForce * Time.deltaTime);
    }

    private void Flip(bool flipToRight)
    {
        float scaleY = flipToRight ? 1f : -1f;
        if (transform.localScale.y != scaleY)
        {
            transform.localScale = new Vector3(1f, scaleY, 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, flipToRight ? 0f : 180f);
        }
    }
}

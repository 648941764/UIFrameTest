using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f; // 角色移动速度
    public float jumpForce = 5.0f; // 跳跃力度
    private bool isJumping = false; // 角色是否正在跳跃
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        // 控制角色左右移动
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        // 如果角色在地面上并且按下跳跃键，则使角色跳跃
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    // 如果角色接触到地面，则可以再次跳跃
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f; // ��ɫ�ƶ��ٶ�
    public float jumpForce = 5.0f; // ��Ծ����
    private bool isJumping = false; // ��ɫ�Ƿ�������Ծ
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        // ���ƽ�ɫ�����ƶ�
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        // �����ɫ�ڵ����ϲ��Ұ�����Ծ������ʹ��ɫ��Ծ
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    // �����ɫ�Ӵ������棬������ٴ���Ծ
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}

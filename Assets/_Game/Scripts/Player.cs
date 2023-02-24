using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;



    private bool isAttack;
    private bool isDeath;

    public float horizontal;

    private int coin = 0;

    private Vector3 savePoint;


    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    void Update()
    {
        isGrounded = CheckGrounded();

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        // Jumping
        Jump();

        if (isJumping || isFalling)
        {
            return;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            Attack();
        }
        // Throw
        if (Input.GetKeyDown(KeyCode.V) && isGrounded)
        {
            Throw();
        }

    }
    void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }
        isGrounded = CheckGrounded();
        
        horizontal = Input.GetAxisRaw("Horizontal");
        // Falling
        Falling();

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isJumping || isFalling)
        {
            return;
        }

        //Moving
        Moving();

        // Attack
        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            Attack();
        }
        // Throw
        if (Input.GetKeyDown(KeyCode.V) && isGrounded)
        {
            Throw();
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        //SavePoint();
        isDeath = false;
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("Idle");
        DeActiveAttack();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        //base.OnDeath();
        ChangeAnim("Die");
        isDeath = true;

        Invoke(nameof(OnInit), 1f);

    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        isFalling = false;

        return hit.collider != null;
    }
    private void Moving()
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            ChangeAnim("Run");
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if (isGrounded)
        {
            ChangeAnim("Idle");
            rb.velocity = Vector2.zero;
        }
    }
    private void Attack()
    {
        ChangeAnim("Attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        Invoke(nameof(ActiveAttack), 0.4f);
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    private void Throw()
    {
        ChangeAnim("Throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }
    private void ResetAttack()
    {
        isAttack = false;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            ChangeAnim("Jump");
            rb.AddForce(jumpForce * Vector2.up);
        }
    }
    private void Falling()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
            ChangeAnim("Fall");
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            }
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(other.gameObject);
        }
        if (other.tag == "DeathZone")
        {
            
            ChangeAnim("Die");
            isDeath = true;

            Invoke(nameof(OnInit), 1f);
        }
    }
}

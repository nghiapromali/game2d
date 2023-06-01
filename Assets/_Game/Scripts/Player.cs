using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 250;
    [SerializeField] private float JumpForce = 350;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;



    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;

    private float horizontal;


    private int coin = 0;

    private Vector3 savePoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }


    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            return;
        }


        isGrounded = CheckGrounded();

        //horizontal = Input.GetAxisRaw("Horizontal"); //nhận đầu vào trục nằm ngang  -1 | 0 | 1 
        //vertical = Input.GetAxisRaw("Vertical");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        

        if (isGrounded)
        {

            if (isJumping)
            {
                return;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //nhận phím space được ấn xuống và player phải ở trên mặt đất -> player jump
            {
                Jump();
            }
            
            //anim run
            if (Mathf.Abs(horizontal) > 0.1f) //khi nhảy k được chuyển sang anim run
            {
                ChangeAnim("run");
            }
            //atack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            //throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        //isfalling
        if (!isGrounded && rb.velocity.y < 0) 
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //moving
        if (Mathf.Abs(horizontal) > 0.1f) //biến float horizontal không ss == 0, != 0 (device sẽ k chuẩn) -> sử dụng giá trị tuyệt đối 
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); 

            //nếu horizontal > 0 -> trả về 0 | nếu horizontal <= 0 -> trả về 180 
            transform.rotation = Quaternion.Euler(new Vector3(0,horizontal >0 ? 0 : 180,0));
            //transform.localScale = new Vector3(horizontal, 1 , 1);
        }
        //idle
        else if (isGrounded) 
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }

    public override void OnInit() //reset thông số đưa về trạng thái ban đầu
    {
        base.OnInit(); //gọi OnInit từ lớp cha
        isAttack = false;

        transform.position = savePoint; //vị trí player = vị trí save point
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        /*savePoint = transform.position;*/ //lưu vị trí savepoint khi chạy

        UIManager.instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }


    private bool CheckGrounded() //kiểm tra xem player có ở trên ground không 
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        //if (hit.collider != nul)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return hit.collider != null; //trả về true or false nếu player ở trên ground hoặc không
    }

    public void Attack() 
    {
        if(isAttack == false && isGrounded)
        {
            ChangeAnim("attack");
            isAttack = true;
            Invoke(nameof(ResetAttack), 0.5f);
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);
        }
    }
    public void Throw()
    {
        if(isAttack == false)
        {
            ChangeAnim("throw");
            isAttack = true;
            Invoke(nameof(ResetAttack), 1.0f);
            Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
        }
     }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }
    public void Jump()
    {
        if (isJumping == false && isGrounded == true)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(JumpForce * Vector2.up);
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

    //player va chạm với coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin") 
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "deathzone")
        {
            ChangeAnim("die");

            Invoke(nameof(OnInit) , 1f);
        }
    }
    
    
    
}

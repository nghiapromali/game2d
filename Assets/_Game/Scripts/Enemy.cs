using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject attackArea;




    private IState currentState; //quy ước chuẩn của state | currentstate(state hiện hành) để kiểm tra.

    private bool isRight = true; //hướng sang phải = true

    private Character target;
    public Character Target => target;

    private void Update()
    {
        if (currentState != null && !IsDead) //nếu state hiện hành khác null
        {
            currentState.OnExecute(this); //update state hiện hành
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        ChangeState(new IdleState());
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }


    public void ChangeState(IState newState) //quy ước chuẩn của state
    {
        //khi đổi sang state mới kiểm tra state cũ có bằng null hay không
        if (currentState != null)
        {
            currentState.OnExit(this); //thoát state cũ đi
        }

        currentState = newState; //gán state mới bằng current state

        if (currentState != null) //kiểm tra currentstate khác null 
        {
            currentState.OnEnter(this); //truy cập vào state mới
        }
    }


    internal void SetTarget(Character character)
    {
        this.target = character;

        if (IstargetInRange()) {
            ChangeState(new AttackState());
        } 
        else
        if (Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
         rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public bool IstargetInRange() //xem mục tiêu có trong tầm đánh không
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)//target khác null và vị trí mục tiêu <= vị trí enemy trả về true và ngược lại 
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            changeDirection(!isRight);
        }
    }

    public void changeDirection(bool isRight)
    {
        this.isRight = isRight; //this trả về isRight gốc | isRight là biến local nên không thay đổi biến bên ngoài
        
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }


}

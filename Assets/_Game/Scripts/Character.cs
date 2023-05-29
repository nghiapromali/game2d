using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText CombatTextPrefab;

    private float hp;

    private string currentAnimName; //anim hiện hành


    public bool IsDead => hp <= 0; //(property) Điều kiện kiểm tra là "hp <= 0" thì IsDead trả về true và ngược lại trả về false

    private void Start()
    {
        OnInit();
    }


    //các hàm kế thừa:

    public virtual void OnInit() // * Hàm tự viết ra để chủ động gọi nó * vì hàm khởi tạo chỉ được gọi 1 lần duy nhất
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDespawn() //tương tự hàm huỷ, khi không cần dùng nữa ta gọi OnDespawn
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }


    public void OnHit(float damage)
    {
        if (!IsDead) //(false) nếu character chưa chết thì hp = hp - damage
        {
            hp -= damage;
            if (IsDead) //(true) nếu nhân vật chết gọi hàm OnDeath() 
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(CombatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage); //Prefab - vị trí - góc xoay 
        }
    }


    
}

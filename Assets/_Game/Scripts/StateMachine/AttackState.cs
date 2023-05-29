using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class AttackState : IState
{
    float timer;
    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            //đổi hướng enemy tới hướng của player
            enemy.changeDirection(enemy.Target.transform.position.x > enemy.transform.position.x); //check hướng 1 lần nữa khi chạm vào đoạn tấn công
            enemy.StopMoving();
            enemy.Attack();

        }
        timer = 0;


    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f) {
            enemy.ChangeState(new PatrolState());
        }

    }

    public void OnExit(Enemy enemy)
    {

    }

}
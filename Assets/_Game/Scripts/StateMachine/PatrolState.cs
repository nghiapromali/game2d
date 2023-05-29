using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float randomTime;
    float timer;
    public void OnEnter(Enemy enemy)
    {
        timer = 0; //reset lại thời gian
        randomTime = Random.Range(3f, 6f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        

        if (enemy.Target !=  null) //nếu mục tiêu của enemy khác null
        {
            enemy.changeDirection(enemy.Target.transform.position.x > enemy.transform.position.x); //so sánh trục x của player. x player > enemy -> hướng sang phải và ngược lại

            if (enemy.IstargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }

        }
        else
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {

    }

}

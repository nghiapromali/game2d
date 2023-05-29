using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision) //khi va chạm
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(collision.GetComponent<Character>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //khi thoát va chạm
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(null);
        }
    }
}

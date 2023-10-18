using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextEnemyTrigger : MonoBehaviour
{
    [SerializeField] Enemy thisEnemy;

    void Start()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //if there is another car ahead of this one: make speeds of this car equal to speed of next one
            Enemy nextEnemy = collision.gameObject.GetComponent<Enemy>();
            thisEnemy.speed = nextEnemy.speed;
        }
    }
}

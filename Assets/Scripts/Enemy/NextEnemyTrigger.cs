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
            Enemy nextEnemy = collision.gameObject.GetComponent<Enemy>();
            thisEnemy.speed = nextEnemy.speed;
        }
    }
}

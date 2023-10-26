using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public int scoreAmountToAdd;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody rb;

    //local
    Vector3 movementVelocity;

    void Update()
    {
        movementVelocity = transform.forward * speed;
        rb.velocity = movementVelocity;
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("CarDestroyTrigger"))
        {
            Destroy(gameObject);
        }
    }
}

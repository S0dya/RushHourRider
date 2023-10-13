using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float speed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody rb;


    //local
    Vector3 movementVelocity;


    void Start()
    {

    }

    void Update()
    {
        movementVelocity = transform.forward * speed * Time.deltaTime;
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
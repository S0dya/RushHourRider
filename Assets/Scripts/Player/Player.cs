using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SingletonMonobehaviour<Player>
{
    Inputs inputs;
    Rigidbody rb;

    [Header("Settings")]
    public float movementSpeed;
    public float sensitivity;

    [Header("SerializeFields")]

    //local
    Vector2 touchStartPos;
    Vector3 inputDirection;

    Vector3 movementVelocity;

    Coroutine movementInputCor;

    protected override void Awake()
    {
        base.Awake();

        inputs = new Inputs();
        rb = GetComponent<Rigidbody>();

    }
    void OnEnable()
    {
        inputs.Enable();
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    void Start()
    {
        inputs.Touch.Touch.started += context => StartTouch(context);
        inputs.Touch.Touch.canceled += context => EndTouch(context);
    }

    void Update()
    {
        movementVelocity = (transform.forward + inputDirection) * movementSpeed * Time.deltaTime;
        rb.velocity = movementVelocity;
    }

    

    //input
    void StartTouch(InputAction.CallbackContext context)
    {
        touchStartPos = context.ReadValue<Vector2>();
        movementInputCor = StartCoroutine(MovementInputCor());
    }
    
    void EndTouch(InputAction.CallbackContext context)
    {
        if (movementInputCor != null) StopCoroutine(movementInputCor);
        inputDirection = Vector2.zero;
    }
    
    IEnumerator MovementInputCor()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 delta = mousePos - touchStartPos;
                inputDirection = new Vector3(delta.x, 0, 0);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)Input.mousePosition - touchStartPos;
                inputDirection = new Vector3(delta.x * sensitivity, 0, 0);
            }
            else
            {
                break;
            }

            yield return null;  
        }

        inputDirection = Vector2.zero; //delater
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Die");
        }
    }


}

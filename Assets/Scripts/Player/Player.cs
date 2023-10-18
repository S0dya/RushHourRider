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

    public float bikeMaxRotation;

    [Header("SerializeFields")]
    [SerializeField] Transform bikeTransform;

    //local
    float touchStartPos;
    float inputDirection;
    Vector3 movementVelocity;

    bool isInput;

    Coroutine increaseSpeedCor;

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

        increaseSpeedCor = StartCoroutine(IncreaseSpeedCor());
    }

    void Update()
    {
        if (isInput)
        {
            if (Input.touchCount > 0)
            {
                //Vector2 mousePos = Input.mousePosition;
                //inputDirection = mousePos.x - touchStartPos;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)Input.mousePosition - new Vector2(touchStartPos, 0);
                float deltaX = delta.normalized.x;
                inputDirection = deltaX * sensitivity;

                bikeTransform.localRotation = Quaternion.Euler(0, bikeMaxRotation * deltaX, 0);
            }
        }

        movementVelocity = transform.forward * movementSpeed;
        movementVelocity.x += inputDirection;
        rb.velocity = movementVelocity;
    }

    

    //input
    void StartTouch(InputAction.CallbackContext context)
    {
        touchStartPos = context.ReadValue<Vector2>().x;
        isInput = true;
    }

    void EndTouch(InputAction.CallbackContext context)
    {
        isInput = false;
        touchStartPos = 0;
    }
    
    //Cors
    IEnumerator IncreaseSpeedCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(movementSpeed * 1.2f);
            movementSpeed++;
        }
    }

    //triger
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Die");
        }
    }

    
}

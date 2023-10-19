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

    public float maxAdditionalSpeed;
    public float zOfCameraDiviser;

    public float boostDuration;

    [Header("SerializeFields")]
    [SerializeField] Transform bikeTransform;
    [SerializeField] Transform playerCameraTransform;

    //local
    float touchStartPos;
    float inputDirection;
    Vector3 movementVelocity;

    bool isInput;
    bool hasShield;

    float additionalSpeed;

    Coroutine increaseSpeedCor;
    Coroutine boostSpeedCor;
    Coroutine boostSpeedStopCor;

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
        //check input
        if (isInput)
        {
            if (Input.touchCount > 0)
            {
                //delta is needed for correct work of direction of input
                Vector2 delta = (Vector2)Input.mousePosition - new Vector2(touchStartPos, 0);
                //we only need x since we are moving player on left or right
                float deltaX = delta.normalized.x;
                inputDirection = deltaX * sensitivity;

                //rotate bike for input visualisation
                bikeTransform.localRotation = Quaternion.Euler(0, bikeMaxRotation * deltaX, 0);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)Input.mousePosition - new Vector2(touchStartPos, 0);
                float deltaX = delta.normalized.x;
                inputDirection = deltaX * sensitivity;

                bikeTransform.localRotation = Quaternion.Euler(0, bikeMaxRotation * deltaX, 0);
            }
        }

        //add force to rigidbody of player to move
        movementVelocity = transform.forward * (movementSpeed + additionalSpeed);
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
            float newSpeed = movementSpeed + 1;
            while (movementSpeed < newSpeed)
            {
                //smoothly increase speed of player
                movementSpeed = Mathf.Lerp(movementSpeed, 13, 0.005f);

                yield return null;
            }
        }
    }

    IEnumerator BoostSpeedCor()
    {
        //increase additional speed smoothly 
        while (additionalSpeed < maxAdditionalSpeed)
        {
            additionalSpeed = Mathf.Lerp(additionalSpeed, 13, 0.005f);
            //smoothly move camera back for speed visualisation
            playerCameraTransform.localPosition = new Vector3(0, 0, -additionalSpeed / zOfCameraDiviser);

            yield return null;
        }

        //wait for boost to end
        yield return new WaitForSeconds(boostDuration);
        //decrease additional speed
        boostSpeedStopCor = StartCoroutine(BoostSpeedStopCor());

        boostSpeedCor = null;
    }
    IEnumerator BoostSpeedStopCor()
    {
        //decrease additional speed smoothly 
        while (additionalSpeed > 0)
        {
            additionalSpeed = Mathf.Lerp(additionalSpeed, -3, 0.01f);
            playerCameraTransform.localPosition = new Vector3(0, 0, -additionalSpeed/zOfCameraDiviser);

            yield return null;
        }

        boostSpeedStopCor = null;
    }


    //triger
    void OnTriggerEnter(Collider collision)
    {
        string tag = collision.tag;
        if (tag == "Obstacle" || tag == "Enemy")
        {
            if (hasShield)
            {
                Debug.Log("breakshield");
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Die");
            }
        }
        else if (collision.gameObject.layer == 8)
        {
            if (tag == "Boost")
            {
                //add additional speed as a response for taking a boost if boost is already being used
                if (boostSpeedCor != null)
                {
                    additionalSpeed += 2.5f;
                }
                else
                {
                    //stop decreasing boost to properly increase boost
                    if (boostSpeedStopCor != null) StopCoroutine(boostSpeedStopCor);
                    boostSpeedCor = StartCoroutine(BoostSpeedCor());
                }
            }
            else if (tag == "Shield")
            {
                hasShield = true;
                Debug.Log("addShield");
            }

            Destroy(collision.gameObject);
        }

    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SingletonMonobehaviour<Player>
{
    Inputs inputs;

    [Header("Settings")]
    public float movementSpeed;
    public float sensitivity;

    public float maxAdditionalSpeed;
    public float zOfCameraDiviser;

    public float boostDuration;
    public float slowmoDuration;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer bikeRenderer;
    [SerializeField] GameMenuUI gameMenu;
    [SerializeField] Transform bikeTransform;
    [SerializeField] Transform playerCameraTransform;

    //local
    float touchStartPos;
    [HideInInspector] public float inputDirection;
    Vector3 movementVelocity;

    bool isInput;
    bool isButtonInput;
    [HideInInspector] public bool isMenuOpened;
    bool hasShield;

    float additionalSpeed;

    Coroutine increaseSpeedCor;
    Coroutine buttonInputCor;
    Coroutine stopButtonInputCor;
    Coroutine boostSpeedCor;
    Coroutine boostSpeedStopCor;
    Coroutine slowTimeCor;
    Coroutine slowTimeStopCor;
    Coroutine scoreMultiplierCor;

    bool touchInput;

    protected override void Awake()
    {
        base.Awake();

        inputs = new Inputs();
        touchInput = Settings.isTouchInput;

        Material[] materials = bikeRenderer.materials;
        materials[0] = GameManager.I.bikeMaterials[Settings.currentColorOfBikeI];
        bikeRenderer.materials = materials;
    }
    void OnEnable()
    {
        if (touchInput) inputs.Enable();
    }
    void OnDisable()
    {
        if (touchInput) inputs.Disable();
    }
    void Start()
    {
        if (touchInput)
        {
            inputs.Touch.Touch.started += context => StartTouch(context);
            inputs.Touch.Touch.canceled += context => EndTouch(context);
        }

        increaseSpeedCor = StartCoroutine(IncreaseSpeedCor());
    }

    void Update()
    {
        if (isMenuOpened) return;
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

            }
            /* mouse input
            else if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)Input.mousePosition - new Vector2(touchStartPos, 0);
                float deltaX = delta.normalized.x;
                inputDirection = deltaX * sensitivity;
            }
            */
        }

        //add force to rigidbody of player to move
        movementVelocity = transform.forward * (movementSpeed + additionalSpeed);
        //rotate bike for input visualisation
        bikeTransform.localRotation = Quaternion.Euler(0, inputDirection, 0);
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

    public void StartButtonInput(int direction)
    {
        if (!isButtonInput)
        {
            isButtonInput = true;
            if (stopButtonInputCor != null) StopCoroutine(stopButtonInputCor);
            buttonInputCor = StartCoroutine(ButtonInputCor(direction));

        }

    }
    public void StopButtonInput(int direction)
    {
        if (buttonInputCor != null) StopCoroutine(buttonInputCor);
        stopButtonInputCor = StartCoroutine(StopButtonInputCor(direction));
     
        isButtonInput = false;
    }


    //Cors
    IEnumerator ButtonInputCor(int direction)
    {
        bool onLeft = direction == -1;
        float curDirection = sensitivity * direction;
        float endDirection = (sensitivity - 0.3f) * direction;
        while (inputDirection > curDirection == onLeft)
        {
            inputDirection = Mathf.Lerp(inputDirection, endDirection, 0.05f);

            yield return null;
        }

        buttonInputCor = null;
    }
    IEnumerator StopButtonInputCor(int direction)
    {
        bool onLeft = direction == -1;
        float endDirection = 0.3f * direction;
        while (inputDirection < 0 == onLeft)
        {
            inputDirection = Mathf.Lerp(inputDirection, endDirection, 0.1f);

            yield return null;
        }

        stopButtonInputCor = null;
    }


    IEnumerator IncreaseSpeedCor()
    {
        while (true)
        {
            float newSpeed = movementSpeed + 1;
            while (movementSpeed < newSpeed)
            {
                //smoothly increase speed of player
                movementSpeed = Mathf.Lerp(movementSpeed, newSpeed + 0.5f, 0.005f);

                yield return null;
            }

            gameMenu.SetSpeedText((int)(movementSpeed + additionalSpeed));
            yield return new WaitForSeconds(movementSpeed * 0.15f);
        }
    }


    //boosts cors
    IEnumerator BoostSpeedCor()
    {
        //show icon of boost
        gameMenu.ToggleBoost(0, true);
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
            playerCameraTransform.localPosition = new Vector3(0, 0, -additionalSpeed /zOfCameraDiviser);

            yield return null;
        }

        gameMenu.ToggleBoost(0, false);
        boostSpeedStopCor = null;
    }

    IEnumerator SlowTimeCor()
    {
        gameMenu.ToggleBoost(2, true);
        while (Settings.currentTimeScale > 0.5f)
        {
            //check if menu is opened (time scale would change if i dont use this kind of logic)
            if (!isMenuOpened) Time.timeScale = Settings.currentTimeScale = Mathf.Lerp(Settings.currentTimeScale, 0.4f, 0.005f);

            yield return null;
        }
        yield return new WaitForSeconds(slowmoDuration);

        slowTimeStopCor = StartCoroutine(SlowTimeStopCor());
        slowTimeCor = null;
    }
    IEnumerator SlowTimeStopCor()
    {
        while (Settings.currentTimeScale < 1f)
        {
            if (!isMenuOpened) Time.timeScale = Settings.currentTimeScale = Mathf.Lerp(Settings.currentTimeScale, 1.1f, 0.05f);

            yield return null;
        }

        gameMenu.ToggleBoost(2, false);
        slowTimeStopCor = null;
    }

    IEnumerator ScoreMultiplierCor()
    {
        gameMenu.scoreMultiplier = 2;
        yield return new WaitForSeconds(10);
        gameMenu.scoreMultiplier = 1;
        gameMenu.ToggleBoost(3, false);
        scoreMultiplierCor = null;
    }


    //triger
    void OnTriggerEnter(Collider collision)
    {
        switch (collision.tag)
        {
            case "Obstacle":
            case "Enemy":
                AudioManager.I.PlayOneShot("Crash");
                if (hasShield)
                {
                    //shield breaks, we just need to remove the icon
                    hasShield = false;
                    gameMenu.ToggleBoost(1, false);
                    Destroy(collision.gameObject);
                }
                else
                {
                    gameMenu.Gameover();
                }
                break;
            case "Boost":
                AudioManager.I.PlayOneShot("BoostPickUp");
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
                break;
            case "Shield":
                if (!hasShield) gameMenu.ToggleBoost(1, true);
                hasShield = true;
                break;
            case "Slowmo":
                if (slowTimeCor != null) return;
                else
                {
                    if (slowTimeStopCor != null) StopCoroutine(slowTimeStopCor);
                    slowTimeCor = StartCoroutine(SlowTimeCor());
                }
                break;
            case "ScoreMultiplier":
                if (scoreMultiplierCor != null) StopCoroutine(scoreMultiplierCor);
                else gameMenu.ToggleBoost(3, true);
                scoreMultiplierCor = StartCoroutine(ScoreMultiplierCor());
                break;
            default: break;
        }

        if (collision.gameObject.layer == 8) Destroy(collision.gameObject);
    }
}

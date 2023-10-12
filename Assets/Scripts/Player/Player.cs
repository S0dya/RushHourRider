using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SingletonMonobehaviour<Player>
{
    Inputs inputs;

    [Header("Settings")]
    [SerializeField] float moveSpeed = 5.0f;

    [Header("SerializeFields")]
    Vector2 touchStartPos;
    bool isTouching = false;

    protected override void Awake()
    {
        base.Awake();

        inputs = new Inputs();

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


    void StartTouch(InputAction.CallbackContext context)
    {
        touchStartPos = context.ReadValue<Vector2>();
        Debug.Log("ASD");
        Debug.Log(touchStartPos);
        isTouching = true;
    }

    void Update()
    {
        if (isTouching)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchCurrentPos = Input.GetTouch(0).position;
                Vector2 touchDelta = touchCurrentPos - touchStartPos;
                Vector3 moveDirection = new Vector3(touchDelta.x, 0, touchDelta.y);
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            else if (Input.GetMouseButton(0)) // Mouse input (left button)
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 touchDelta = mousePos - touchStartPos;
                Vector3 moveDirection = new Vector3(touchDelta.x, 0, touchDelta.y);
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
    }

    void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("123");
        isTouching = false;
    }
}

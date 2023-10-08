using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputFacade : MonoBehaviour, IInputFacade
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private JumpButtonHandler jumpButton;
    [SerializeField] private bool isMobile;
    [SerializeField] private bool isJumping;
    private float _horizontal;

    private void Start()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        isMobile = false;
        #endif
        #if UNITY_ANDROID || UNITY_IOS
        isMobile = true;
        #endif
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumping = true;
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }
    
    public void HorizontalAxisPC(InputAction.CallbackContext context)
    {
        //get value
        _horizontal = context.ReadValue<Vector2>().x;
    }


    public float HorizontalAxis => isMobile ? joystick.Horizontal : _horizontal;
    public bool JumpButton => isMobile ? jumpButton.IsJumpButtonDown() : isJumping;
}
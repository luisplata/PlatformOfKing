using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputFacade : MonoBehaviour, IInputFacade
{
    [SerializeField] private Joystick joystick;
    [FormerlySerializedAs("jumpButton")] [SerializeField] private CustomButtonHandler customButton;
    [SerializeField] private CustomButtonHandler actionButton;
    [SerializeField] private bool isMobile;
    [SerializeField] private bool isJumping, isAction;
    private float _horizontal;

    private void Start()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        isMobile = false;
        #elif UNITY_ANDROID || UNITY_IOS
        isMobile = true;
        #endif

        joystick.gameObject.SetActive(isMobile);
        customButton.gameObject.SetActive(isMobile);
        actionButton.gameObject.SetActive(isMobile);
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
    public void Action(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAction = true;
        }
        else if (context.canceled)
        {
            isAction = false;
        }
    }
    
    public void HorizontalAxisPC(InputAction.CallbackContext context)
    {
        //get value
        _horizontal = context.ReadValue<Vector2>().x;
    }
    
    public float HorizontalAxis => isMobile ? joystick.Horizontal : _horizontal;
    public bool JumpButton => isMobile ? customButton.IsButtonDown() : isJumping;
    public bool ActionButton => isMobile ? actionButton.IsButtonDown() : isAction;
}
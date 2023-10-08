using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isJumping;

    public void OnPointerDown(PointerEventData eventData)
    {
        isJumping = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isJumping = false;
    }

    public bool IsJumpButtonDown()
    {
        return isJumping;
    }
}
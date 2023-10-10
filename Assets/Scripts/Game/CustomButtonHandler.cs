using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isActioning;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isActioning = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isActioning = false;
    }

    public bool IsButtonDown()
    {
        return _isActioning;
    }
}
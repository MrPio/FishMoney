using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

internal enum InputType
{
    Hook,
    Bomb
}

public class InputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private InputType inputType;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inputType is InputType.Hook) InputManager.Instance.IsHookDown = true;
        if (inputType is InputType.Bomb) InputManager.Instance.IsBombDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (inputType is InputType.Hook) InputManager.Instance.IsHookDown = false;
        if (inputType is InputType.Bomb) InputManager.Instance.IsBombDown = false;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inputType is InputType.Hook) InputManager.Instance.IsHookDown = false;
        if (inputType is InputType.Bomb) InputManager.Instance.IsBombDown = false;
    }
}
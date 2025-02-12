using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class InputsMenu : MonoBehaviour
{
    [SerializeField] private Sprite hookInputKeyboard, bombInputKeyboard, hookInputJoystick, bombInputJoystick;
    [SerializeField] private Image hookInput, bombInput;

    private void OnEnable()
    {
        hookInput.sprite = InputManager.Instance.HasJoystick() ? hookInputJoystick : hookInputKeyboard;
        bombInput.sprite = InputManager.Instance.HasJoystick() ? bombInputJoystick : bombInputKeyboard;
    }
}
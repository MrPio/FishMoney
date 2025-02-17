﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionsFunctions;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace Managers
{
    public class InputManager
    {
        public static void Reset() => _instance = new InputManager();

        private static InputManager _instance;

        private InputManager()
        {
        }

        public static InputManager Instance => _instance ??= new InputManager();

        public static bool IsMobile =>
            Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;


        private bool? _hasJoystick;

        public bool IsHookDown, IsBombDown, IsPauseDown;

        public bool HasJoystick() => _hasJoystick ??= Gamepad.current is not null;

        public bool GetHookDown() =>
            IsMobile
                ? IsHookDown.GetAsTrigger()
                : HasJoystick()
                    ? Input.GetKey(KeyCode.Joystick1Button0)
                    : Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.DownArrow);

        public bool GetBombDown() =>
            IsMobile
                ? IsBombDown.GetAsTrigger()
                : HasJoystick()
                    ? Input.GetKeyDown(KeyCode.Joystick1Button1)
                    : Input.GetKeyDown(KeyCode.UpArrow);

        public bool GetPauseDown() =>
            IsMobile
                ? IsPauseDown.GetAsTrigger()
                : HasJoystick()
                    ? Input.GetKeyDown(KeyCode.Joystick1Button6)
                    : Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);


        public IEnumerator Vibrate(float duration = 0.35f)
        {
            if (HasJoystick())
            {
                Gamepad.current.SetMotorSpeeds(0.5f, 1f);
                yield return new WaitForSeconds(duration);
                Gamepad.current.SetMotorSpeeds(0f, 0f);
            }
            else if (IsMobile)
            {
                // Handheld.Vibrate();
            }
        }
    }
}
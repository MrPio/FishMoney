using System.Collections;
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

        public bool IsHookDown, IsBombDown;

        public bool HasJoystick() => _hasJoystick ??= Gamepad.current is not null;

        public bool GetHookDown() =>
            IsMobile
                ? IsHookDown.GetAsTrigger()
                : HasJoystick()
                    ? Input.GetKeyDown(KeyCode.Joystick1Button0)
                    : Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.DownArrow);

        public bool GetBombDown() =>
            IsMobile
                ? IsBombDown.GetAsTrigger()
                : HasJoystick()
                    ? Input.GetKeyDown(KeyCode.Joystick1Button1)
                    : Input.GetKeyDown(KeyCode.UpArrow);


        public IEnumerator Vibrate(float duration = 0.35f)
        {
            if (HasJoystick())
            {
                Gamepad.current.SetMotorSpeeds(1f, 1f);
                yield return new WaitForSeconds(duration);
                Gamepad.current.ResetHaptics();
            }
            else if (IsMobile)
            {
                // Handheld.Vibrate();
            }
        }
    }
}
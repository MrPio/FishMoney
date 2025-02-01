using System;
using System.Collections;
using UnityEngine;

namespace Prefabs
{
    internal enum HookState
    {
        None,
        Idle,
        Retreating,
        Launching
    }

    public class Hook : MonoBehaviour
    {
        private static readonly int
            PlayerIdle = Animator.StringToHash("idle"),
            PlayerLaunch = Animator.StringToHash("launch"),
            PlayerTurn = Animator.StringToHash("turn"),
            WheelIdle = Animator.StringToHash("idle"),
            WheelLaunch = Animator.StringToHash("launch"),
            WheelTurn = Animator.StringToHash("turn"),
            HookIdle = Animator.StringToHash("idle"),
            HookLaunch = Animator.StringToHash("launch"),
            HookRetreat = Animator.StringToHash("retreat"),
            HookGrab = Animator.StringToHash("grab");

        [SerializeField] private float
            rotationSpeed = 0.3f,
            maxRotation = 50f,
            reach = 11f,
            launchDuration = 1.2f,
            upFactor = 0.5f;

        [SerializeField] private Transform itemHolder;
        [SerializeField] private AnimationCurve rotationCurve, launchCurve, reachCurve;
        [SerializeField] private Animator hookAnimator, playerAnimator, wheelAnimator;

        private GameManager _gm;
        private float _launchStart, _retreatStart, _acc, _actualReach, _rotX;
        private Vector3 _startRotation, _startPosition;
        private HookState _state;
        private Item _grabbedItem;

        private float Reach => reach * reachCurve.Evaluate(_rotX / maxRotation);
        private float UpFactor => upFactor * (_grabbedItem is null ? 1f : _grabbedItem.ItemModel.Weight);

        private void Awake()
        {
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Start()
        {
            _startRotation = transform.localRotation.eulerAngles;
            _startPosition = transform.localPosition;
            _state = HookState.Idle;
        }

        private void Update()
        {
            // Hook is rotating
            if (_state is HookState.Idle)
            {
                _acc += Time.deltaTime;
                _rotX = rotationCurve.Evaluate(_acc * rotationSpeed) * maxRotation;
                transform.localRotation = Quaternion.Euler(
                    x: _rotX,
                    y: _startRotation.y,
                    z: _startRotation.z
                );
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.Space))
                {
                    _launchStart = Time.time;
                    _state = HookState.Launching;
                    playerAnimator.SetTrigger(PlayerLaunch);
                    wheelAnimator.SetTrigger(WheelLaunch);
                    hookAnimator.SetTrigger(HookLaunch);
                }
            }

            // Launching
            if (_state is HookState.Launching)
            {
                var dt = Time.time - _launchStart;
                _actualReach = launchCurve.Evaluate(dt / launchDuration) * Reach;
                transform.localPosition =
                    _startPosition + Quaternion.Euler(0, 0, -90) * transform.forward * _actualReach;

                // Done launching
                if (dt > launchDuration)
                {
                    _retreatStart = Time.time;
                    _state = HookState.Retreating;
                    playerAnimator.SetTrigger(PlayerTurn);
                    wheelAnimator.SetTrigger(WheelTurn);
                    hookAnimator.SetTrigger(HookRetreat);
                }
            }

            // Retreating
            else if (_state is HookState.Retreating)
            {
                var dt = Time.time - _retreatStart;
                var dx = (1 - dt /
                    (launchDuration * UpFactor * _actualReach / Reach)) * _actualReach;
                transform.localPosition = _startPosition + Quaternion.Euler(0, 0, -90) * transform.forward * dx;

                // Done retreating
                if (dt > launchDuration * UpFactor * _actualReach / Reach)
                {
                    transform.localPosition = _startPosition;
                    _state = HookState.None;
                    playerAnimator.SetTrigger(PlayerIdle);
                    wheelAnimator.SetTrigger(WheelIdle);
                    hookAnimator.SetTrigger(HookIdle);
                    StartCoroutine(GotoIdle());

                    IEnumerator GotoIdle()
                    {
                        yield return new WaitForSeconds(0.25f);
                        _acc += 0.25f;
                        _state = HookState.Idle;

                        // Item collection
                        if (_grabbedItem is not null)
                            CollectItem();
                    }
                }
            }
        }

        private void CollectItem()
        {
            _gm.CollectItem(_grabbedItem.ItemModel);
            Destroy(_grabbedItem.gameObject);
            _grabbedItem = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_state is HookState.Launching && other.transform.parent.gameObject.CompareTag("Item"))
            {
                _retreatStart = Time.time;
                _state = HookState.Retreating;
                _grabbedItem = other.gameObject.transform.parent.GetComponent<Item>();
                _grabbedItem.transform.SetParent(itemHolder);
                _grabbedItem.transform.localPosition = Vector3.zero;
                _grabbedItem.transform.Rotate(Vector3.back, transform.localRotation.eulerAngles.x);
                playerAnimator.SetTrigger(PlayerTurn);
                wheelAnimator.SetTrigger(WheelTurn);
                hookAnimator.SetTrigger(HookGrab);
            }
        }
    }
}
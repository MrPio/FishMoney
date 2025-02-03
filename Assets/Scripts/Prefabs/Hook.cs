using System.Collections;
using Managers;
using Model;
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

    [RequireComponent(typeof(AudioSource))]
    public class Hook : MonoBehaviour
    {
        private static readonly int
            PlayerIdle = Animator.StringToHash("idle"),
            PlayerLaunch = Animator.StringToHash("launch"),
            PlayerTurn = Animator.StringToHash("turn"),
            PlayerThrow = Animator.StringToHash("throw"),
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
        [SerializeField] private AudioClip launchClip, retreatClip, itemPickupClip, freeHookClip, noBomb;
        [SerializeField] private float retreatClipDelay = 1f;

        private GameManager _gm;
        private float _launchStart, _retreatStart, _acc, _actualReach, _rotX, _lastBombThrown;
        private Vector3 _startRotation, _startPosition;
        private HookState _state;
        private Item _grabbedItem;
        private AudioSource _audioSource;

        private float Reach => reach * reachCurve.Evaluate(_rotX / maxRotation);
        private float UpFactor => upFactor * (_grabbedItem is null ? 1f : _grabbedItem.ItemModel.Weight);
        private float RotationSpeed => rotationSpeed * (_gm.HasRotation ? 1.4f : 1);
        private float LaunchDuration => launchDuration * (_gm.HasWater ? 0.75f : 1);

        private void Awake()
        {
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _audioSource = GetComponent<AudioSource>();
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
                _rotX = rotationCurve.Evaluate(_acc * RotationSpeed) * maxRotation;
                transform.localRotation = Quaternion.Euler(
                    x: _rotX,
                    y: _startRotation.y,
                    z: _startRotation.z
                );
                if (InputManager.Instance.GetHookDown())
                {
                    _launchStart = Time.time;
                    _state = HookState.Launching;
                    playerAnimator.SetTrigger(PlayerLaunch);
                    wheelAnimator.SetTrigger(WheelLaunch);
                    hookAnimator.SetTrigger(HookLaunch);
                    _audioSource.PlayOneShot(launchClip);
                }
            }

            // Launching
            if (_state is HookState.Launching)
            {
                var dt = Time.time - _launchStart;
                _actualReach = launchCurve.Evaluate(dt / LaunchDuration) * Reach;
                transform.localPosition =
                    _startPosition + Quaternion.Euler(0, 0, -90) * transform.forward * _actualReach;

                // Done launching
                if (dt > LaunchDuration)
                {
                    _retreatStart = Time.time;
                    _state = HookState.Retreating;
                    playerAnimator.SetTrigger(PlayerTurn);
                    wheelAnimator.SetTrigger(WheelTurn);
                    hookAnimator.SetTrigger(HookRetreat);
                    StartCoroutine(PlayRetreatClip());
                }
            }

            // Retreating
            else if (_state is HookState.Retreating)
            {
                var dt = Time.time - _retreatStart;
                var dx = (1 - dt /
                    (LaunchDuration * UpFactor * _actualReach / Reach)) * _actualReach;
                transform.localPosition = _startPosition + Quaternion.Euler(0, 0, -90) * transform.forward * dx;

                // Done retreating
                if (dt > LaunchDuration * UpFactor * _actualReach / Reach)
                {
                    transform.localPosition = _startPosition;
                    _state = HookState.None;
                    playerAnimator.SetTrigger(PlayerIdle);
                    wheelAnimator.SetTrigger(WheelIdle);
                    hookAnimator.SetTrigger(HookIdle);
                    StartCoroutine(GotoIdle());

                    IEnumerator GotoIdle()
                    {
                        yield return new WaitForSeconds(0.1f);
                        _acc += 0.1f;
                        _state = HookState.Idle;

                        // Item collection
                        if (_grabbedItem is not null)
                            CollectItem();
                    }
                }
            }

            // Bomb input
            if (InputManager.Instance.GetBombDown())
            {
                if (_gm.Bombs > 0 && Time.time - _lastBombThrown > 1.75 &&
                    (Time.time - _retreatStart) / (LaunchDuration * UpFactor * _actualReach / Reach) < 0.9)
                {
                    _lastBombThrown = Time.time;
                    playerAnimator.SetTrigger(PlayerThrow);
                    _gm.Bombs -= 1;
                }
                else
                {
                    _audioSource.PlayOneShot(noBomb);
                }
            }
        }

        private IEnumerator PlayRetreatClip()
        {
            while (_state is HookState.Retreating)
            {
                yield return new WaitForSeconds(retreatClipDelay / 2);
                _audioSource.PlayOneShot(retreatClip);
                yield return new WaitForSeconds(retreatClipDelay / 2);
            }
        }

        private void CollectItem()
        {
            if (_gm.CollectItem(_grabbedItem.ItemModel))
                _audioSource.PlayOneShot(itemPickupClip);
            Destroy(_grabbedItem.gameObject);
            _grabbedItem = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_state is HookState.Launching && other.transform.parent.gameObject.CompareTag("Item"))
            {
                // Done launching
                _retreatStart = Time.time;
                _state = HookState.Retreating;
                _grabbedItem = other.gameObject.transform.parent.GetComponent<Item>();
                _grabbedItem.StopFlying();
                _grabbedItem.transform.SetParent(itemHolder);
                _grabbedItem.transform.localPosition = Vector3.zero;
                _grabbedItem.transform.Rotate(Vector3.back, transform.localRotation.eulerAngles.x);
                playerAnimator.SetTrigger(PlayerTurn);
                wheelAnimator.SetTrigger(WheelTurn);
                hookAnimator.SetTrigger(HookGrab);
                StartCoroutine(PlayRetreatClip());
                _audioSource.PlayOneShot(Resources.Load<AudioClip>(_grabbedItem.ItemModel.Clip));
                if (_grabbedItem.ItemModel.Type is ItemType.Explosive)
                    _grabbedItem.Explode();
            }
        }

        public void Free()
        {
            if (_state is HookState.Retreating)
            {
                var dt = Time.time - _retreatStart;
                var percent = dt / (LaunchDuration * UpFactor * _actualReach / Reach);
                Destroy(_grabbedItem.gameObject);
                _grabbedItem = null;
                _retreatStart = Time.time - percent * LaunchDuration * UpFactor * _actualReach / Reach;
                _audioSource.PlayOneShot(freeHookClip);
            }
        }
    }
}
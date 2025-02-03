using System;
using System.Linq;
using Managers;
using Model;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prefabs
{
    public class Item : MonoBehaviour
    {
        private readonly float _flyRange = 12;
        private readonly Vector2 _birdSpeedRange = new(2, 4);
        private readonly Vector2 _birdDiamondSpeedRange = new(3, 5);

        [SerializeField] private Vector2 idleAnimationFrequencyRange = new(2, 7);
        [SerializeField] private ItemType itemType;
        [SerializeField] private GameObject explosion, explodedMesh;
        [SerializeField] private float explosionRange = 2.75f;

        private static readonly int Idle = Animator.StringToHash("idle");
        private GameManager _gm;
        private Animator _animator;
        private float _idleAcc, _delay, _flyingAcc;
        private bool _hasAnimator = false;
        private float _flyingSpeed;
        private bool _isFlyingLeft = true;
        public Model.Item ItemModel => Model.Item.Items.First(it => it.Type == itemType);

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _hasAnimator = _animator != null;
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _delay = Random.Range(idleAnimationFrequencyRange.x, idleAnimationFrequencyRange.y) / 2;
        }

        private void Start()
        {
            if (itemType is ItemType.Bird) _flyingSpeed = Random.Range(_birdSpeedRange.x, _birdSpeedRange.y);
            if (itemType is ItemType.BirdDiamond)
                _flyingSpeed = Random.Range(_birdDiamondSpeedRange.x, _birdDiamondSpeedRange.y);
        }

        private void FixedUpdate()
        {
            if (_hasAnimator)
            {
                _idleAcc += Time.deltaTime;
                if (_idleAcc > _delay)
                {
                    _animator.SetTrigger(Idle);
                    _delay = Random.Range(idleAnimationFrequencyRange.x, idleAnimationFrequencyRange.y);
                    _idleAcc = 0;
                }
            }

            if (itemType is ItemType.Bird or ItemType.BirdDiamond)
            {
                transform.position += (_isFlyingLeft ? Vector3.left : Vector3.right) * (Time.deltaTime * _flyingSpeed);
                if (Mathf.Abs(transform.position.x) > _flyRange)
                {
                    transform.Rotate(Vector3.up, 180f);
                    _isFlyingLeft = !_isFlyingLeft;
                }
            }
        }

        public void StopFlying()
        {
            _flyingSpeed = 0;
        }

        public void Explode()
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            transform.GetChild(0).gameObject.SetActive(false);
            explodedMesh.SetActive(true);
            foreach (var item in GameObject.FindGameObjectsWithTag("Item")
                         .Where(it => !it.IsDestroyed() && it != gameObject))
                if (Vector3.Distance(item.transform.position, transform.position) < explosionRange)
                    Destroy(item, 0.1f);
        }
    }
}
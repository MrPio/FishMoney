using System.Linq;
using Managers;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prefabs
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private Vector2 idleAnimationFrequencyRange = new(2, 7);
        [SerializeField] private ItemType itemType;

        private static readonly int Idle = Animator.StringToHash("idle");
        private GameManager _gm;
        private Animator _animator;
        private float _acc, _delay;
        public Model.Item ItemModel => Model.Item.Items.First(it => it.Type == itemType);

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _delay = Random.Range(idleAnimationFrequencyRange.x, idleAnimationFrequencyRange.y);
        }

        private void FixedUpdate()
        {
            if (_animator is null) return;
            _acc += Time.deltaTime;
            if (_acc > _delay)
            {
                _animator.SetTrigger(Idle);
                _delay = Random.Range(idleAnimationFrequencyRange.x, idleAnimationFrequencyRange.y);
                _acc = 0;
            }
        }
    }
}
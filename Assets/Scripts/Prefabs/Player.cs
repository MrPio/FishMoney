using UnityEngine;

namespace Prefabs
{
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject bomb;
        [SerializeField] private Hook hook;
        [SerializeField] private float bombLaunchDuration, bombRotationSpeed;
        [SerializeField] private Transform launchPoint;
        [SerializeField] private AnimationCurve launchCurve;
        [SerializeField] private AudioClip bombThrowClip, explosionClip;
        private float? _bombLaunchStart;
        private Transform _instantiatedBomb;
        private Vector3 _startPoint;
        private AudioSource _audioSource;
        private Vector3 _randomRotationVector;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _randomRotationVector = Random.onUnitSphere;
        }

        private void ThrowBomb()
        {
            _bombLaunchStart = Time.time;
            _instantiatedBomb = Instantiate(bomb).transform;
            _startPoint = launchPoint.position;
            _audioSource.PlayOneShot(bombThrowClip);
        }

        private void Update()
        {
            if (_bombLaunchStart is not null)
            {
                var dt = Time.time - _bombLaunchStart!.Value;
                _instantiatedBomb.transform.position =
                    Vector3.Lerp(_startPoint, hook.transform.position, launchCurve.Evaluate(dt / bombLaunchDuration));
                _instantiatedBomb.transform.rotation =
                    Quaternion.Euler(_randomRotationVector * (Time.time * bombRotationSpeed));
                if (dt > bombLaunchDuration)
                {
                    _bombLaunchStart = null;
                    _instantiatedBomb.GetComponent<Bomb>().Explode();
                    _audioSource.PlayOneShot(explosionClip);
                    hook.Free();
                }
            }
        }
    }
}
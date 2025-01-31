using UnityEngine;

namespace Prefabs
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 0.3f,
            maxRotation = 50f,
            reach = 10f,
            launchDuration = 1.5f,
            upFactor = 0.5f;

        [SerializeField] private AnimationCurve rotationCurve, launchCurve;
        private float? _launchStart = null;
        private Vector3 _startRotation, _startPosition;
        [SerializeField] private Animator hookAnimator, playerAnimator, wheelAnimator;

        private void Start()
        {
            print(transform.rotation.eulerAngles);
            _startRotation = transform.localRotation.eulerAngles;
            _startPosition = transform.localPosition;
        }

        private void Update()
        {
            if (_launchStart == null)
            {
                transform.localRotation = Quaternion.Euler(
                    x: rotationCurve.Evaluate(Time.time * rotationSpeed) * maxRotation,
                    y: _startRotation.y,
                    z: _startRotation.z
                );
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.Space))
                {
                    _launchStart = Time.time;
                }
            }
            else
            {
                // Is launching hook
                var dt = Time.time - _launchStart!.Value;

                // Down
                if (dt < launchDuration)
                {
                    var dx = launchCurve.Evaluate(dt / launchDuration) * reach;
                    transform.localPosition = _startPosition + Quaternion.Euler(0, 0, -90)*transform.forward * dx;
                    hookAnimator.SetTrigger("launch");
                    //TODO define enum di states here
                }
                // Up
                else if (dt < launchDuration * (1 + upFactor))
                {
                    var dx = launchCurve.Evaluate(1 - (dt - launchDuration) / (launchDuration * upFactor)) * reach;
                    transform.localPosition = _startPosition + Quaternion.Euler(0, 0, -90)*transform.forward * dx;
                    hookAnimator.SetTrigger("retire");
                    // hookAnimator.SetTrigger("grab");
                }
                else
                {
                    transform.localPosition = _startPosition;
                    _launchStart = null;
                    hookAnimator.SetTrigger("idle");
                }
            }
        }
    }
}
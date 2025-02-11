using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TargetMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI level, target;
        private GameManager _gm;

        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        private void OnEnable()
        {
            level.text = $"Level {_gm.Level.Id}";
            target.text = "$ " + _gm.Level.Target.ToString("N0");
        }
    }
}
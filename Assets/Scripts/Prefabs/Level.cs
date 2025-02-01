using System.Linq;
using Managers;
using UnityEngine;

namespace Prefabs
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Material skybox;
        [SerializeField] private int id;
        private GameManager _gm;
        public Model.Level LevelModel => Model.Level.Levels.First(it => it.Id == id);

        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Start()
        {
            RenderSettings.skybox = skybox;
        }
    }
}
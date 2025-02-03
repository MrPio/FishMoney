using System.Linq;
using Managers;
using UnityEngine;

namespace Prefabs
{
    public class Level : MonoBehaviour
    {
        [SerializeField] public Material skybox;
        [SerializeField] private int id;
        public Model.Level LevelModel => Model.Level.Levels.First(it => it.Id == id);
    }
}
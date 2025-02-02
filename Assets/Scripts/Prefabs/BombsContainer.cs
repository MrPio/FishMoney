using System.Collections.Generic;
using UnityEngine;

namespace Prefabs
{
    public class BombsContainer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> bombs;

        private void Awake()
        {
            foreach (var bomb in bombs)
                bomb.SetActive(false);
        }

        public void SetBombs(int amount)
        {
            for (var i = 0; i < 10; i++)
                bombs[i].SetActive(i < amount);
        }
    }
}
using UnityEngine;

namespace Prefabs
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private GameObject explosion;

        public void Explode()
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.5f);
        }
    }
}
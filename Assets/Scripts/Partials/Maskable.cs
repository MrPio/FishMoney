using System;
using UnityEngine;

namespace Partials
{
    public class Maskable : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }
}
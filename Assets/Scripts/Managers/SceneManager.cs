using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    [SerializeField] [Range(.1f, 10)] private float skySpeed = .4f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private float _startSkyboxRotation;

    private void Start()
    {
        _startSkyboxRotation = Random.Range(0, 360);
    }

    private void Update () {
        RenderSettings.skybox.SetFloat(Rotation, _startSkyboxRotation+Time.time * skySpeed);
    }
}

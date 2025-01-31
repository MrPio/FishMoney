using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] private float skySpeed = 4.0f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private void Update () {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * skySpeed);
    }
}

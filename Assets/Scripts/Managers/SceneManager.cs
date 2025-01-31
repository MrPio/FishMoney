using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] [Range(.1f, 10)] private float skySpeed = .4f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    private void Update () {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * skySpeed);
    }
}

using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText, scoreText;
    [SerializeField] private Image merchantImage;
    [SerializeField] private List<Sprite> sprites;
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        scoreText.text = $"Score: {_gm.Money}";
        timeText.text = $"Time: {_gm.elapsedTime / 60:N0}:{_gm.elapsedTime % 60:N0}";
        merchantImage.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
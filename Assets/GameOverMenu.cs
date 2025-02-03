using System;
using Managers;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText, scoreText, timeText;
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        levelText.text = $"Level: {_gm.Level.Id}";
        scoreText.text = $"Score: {_gm.Money}";
        timeText.text = $"Time: {Time.time / 60:N0}:{Time.time % 60:N0}";
    }
}
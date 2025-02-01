using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int LevelDuration = 60;
    private int _money;
    private int _level;
    private float _gameStart;
    [SerializeField] private TextMeshProUGUI moneyText, targetText, levelText, timeText;
    private List<Model.Item> _collectedItems = new();
    private static readonly int Set = Animator.StringToHash("set");
    private TextMeshProUGUI _lastEarnText;
    private Animator _lastEarnAnimator;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            moneyText.text = "$ " + _money.ToString("N0");
        }
    }

    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            levelText.text = _level.ToString();
            // TODO targetText.text =
            _gameStart = Time.time;
        }
    }

    private void Awake()
    {
        _lastEarnText = GameObject.FindWithTag("LastEarn").GetComponent<TextMeshProUGUI>();
        _lastEarnAnimator = GameObject.FindWithTag("LastEarn").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        var leftTime = LevelDuration - Time.time - _gameStart;
        timeText.text = math.clamp(leftTime, 0, LevelDuration).ToString("N0");
    }


    public void CollectItem(Model.Item item)
    {
        if (item is null) return;
        _collectedItems.Add(item);
        var value = item.Value;
        //TODO account for perks
        Money += value;
        _lastEarnText.text = $"+${value:N0}";
        _lastEarnAnimator.SetTrigger(Set);
        print($"Collected {item.Type}!");
    }
}
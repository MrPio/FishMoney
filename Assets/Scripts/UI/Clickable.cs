using System;
using Managers;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal enum Buttons
{
    None,
    MainMenuNewGame,
    ShopMenuNextLevel
}

[RequireComponent(typeof(AudioSource))]
public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IPointerDownHandler
{
    [SerializeField] private Sprite baseSprite, hoverSprite, downSprite;
    [SerializeField] private AudioClip hoverClip, clickClip;
    [SerializeField] private Color textHoverColor = Color.white, textDownColor = Color.white;
    private Image _buttonImage;
    private AudioSource _audioSource;
    private TextMeshProUGUI _text;
    [SerializeField] private Buttons button = Buttons.None;
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _buttonImage = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.sprite = hoverSprite;
        _audioSource.PlayOneShot(hoverClip);
        _text.color = textHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImage.sprite = baseSprite;
        _text.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonImage.sprite = baseSprite;
        _audioSource.PlayOneShot(clickClip);
        _text.color = Color.white;

        if (button is Buttons.MainMenuNewGame)
        {
            _gm.LoadLevel(Level.Levels[0]);
        }
        else if (button is Buttons.ShopMenuNextLevel)
        {
            _gm.LoadLevel(Level.Levels[_gm.Level.Id]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonImage.sprite = downSprite;
        _text.color = textDownColor;
    }
}
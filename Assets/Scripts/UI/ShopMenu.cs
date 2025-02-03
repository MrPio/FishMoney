using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ShopItem = Prefabs.ShopItem;

[Serializable]
public class Merchant
{
    public Sprite sprite;
    public Sprite background;
    public Color boxColor;
    public int fromLevel;
    public string welcomeSentence;
}

[RequireComponent(typeof(AudioSource))]
public class ShopMenu : MonoBehaviour
{
    [SerializeField] private List<Merchant> merchants;
    [SerializeField] private TextMeshProUGUI dialogBoxText, moneyText, priceText;
    [SerializeField] private Image backgroundImage, merchantImage, dialogBox;
    [SerializeField] private List<ShopItem> shopItems;
    [SerializeField] private AudioClip itemSelectClip;
    [SerializeField] private AudioClip itemBuyClip, noBuyClip;

    [SerializeField] private Color dialogColor = Color.white, itemInfoColor;
    private GameManager _gm;
    private AudioSource _audioSource;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        LoadShop();
    }

    private void LoadShop()
    {
        var level = _gm.Level ?? Level.Levels[0];
        var merchant = merchants.First(it => it.fromLevel <= level.Id);
        merchantImage.sprite = merchant.sprite;
        dialogBoxText.text = merchant.welcomeSentence;
        dialogBox.color = merchant.boxColor;
        backgroundImage.sprite = merchant.background;
        foreach (var shopItem in shopItems)
            shopItem.gameObject.SetActive(level.ShopItems.ContainsKey(shopItem.shopItemType));
        moneyText.text = $"$ {_gm.Money:N0}";
        SelectItem(null);
    }

    public void SelectItem(ShopItemType? shopItemType)
    {
        var level = _gm.Level ?? Level.Levels[0];
        var merchant = merchants.First(it => it.fromLevel <= level.Id);
        if (shopItemType is null)
        {
            dialogBoxText.text = merchant.welcomeSentence;
            dialogBoxText.color = dialogColor;
            priceText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dialogBoxText.text = Model.ShopItem.ShopItems.First(it => it.Type == shopItemType).Description;
            dialogBoxText.color = itemInfoColor;
            _audioSource.PlayOneShot(itemSelectClip);
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text = $"$ {level.ShopItems[shopItemType!.Value]:N0}";
        }
    }

    public bool BuyItem(ShopItemType shopItemType)
    {
        var level = _gm.Level ?? Level.Levels[0];
        var price = level.ShopItems[shopItemType];
        if (_gm.Money >= price)
        {
            _audioSource.PlayOneShot(itemBuyClip);
            _gm.Money -= price;
            moneyText.text = $"$ {_gm.Money:N0}";
            SelectItem(null);
        }
        else
            _audioSource.PlayOneShot(noBuyClip);

        return _gm.Money >= price;
    }
}
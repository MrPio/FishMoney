using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ShopItem = Prefabs.ShopItem;

namespace UI
{
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

        [SerializeField]
        private Image backgroundImage, merchantImage, merchantTargetMenuImage, dialogBox, dialogBoxTargetMenu;

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
            var merchant = merchants.Last(it => it.fromLevel <= level.Id);
            var merchant2 = merchants.Last(it => it.fromLevel <= level.Id + 1);
            merchantImage.sprite = merchant.sprite;
            merchantTargetMenuImage.sprite = merchant2.sprite;
            dialogBoxText.text = merchant.welcomeSentence;
            dialogBox.color = merchant.boxColor;
            dialogBoxTargetMenu.color = merchant2.boxColor;
            backgroundImage.sprite = merchant.background;
            foreach (var shopItem in shopItems)
            {
                shopItem.gameObject.SetActive(level.ShopItems.ContainsKey(shopItem.shopItemType));
                if (level.ShopItems.ContainsKey(shopItem.shopItemType))
                    shopItem.transform.Find("price").GetComponent<TextMeshProUGUI>().text =
                        level.ShopItems[shopItem.shopItemType].ToString("N0");
            }

            moneyText.text = $"$ {_gm.Money:N0}";
            SelectItem(null);
        }

        public void SelectItem(ShopItemType? shopItemType)
        {
            var level = _gm.Level ?? Level.Levels[0];
            var merchant = merchants.Last(it => it.fromLevel <= level.Id);
            if (shopItemType is null)
            {
                dialogBoxText.text = merchant.welcomeSentence;
                dialogBoxText.color = dialogColor;
                // priceText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                dialogBoxText.text = Model.ShopItem.ShopItems.First(it => it.Type == shopItemType).Description;
                dialogBoxText.color = itemInfoColor;
                _audioSource.PlayOneShot(itemSelectClip);
                // priceText.transform.parent.gameObject.SetActive(true);
                // priceText.text = $"$ {level.ShopItems[shopItemType!.Value]:N0}";
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
                if (shopItemType is ShopItemType.Bomb) _gm.Bombs++;
                if (shopItemType is ShopItemType.Clover) _gm.HasClover = true;
                if (shopItemType is ShopItemType.Magnifier) _gm.Magnifier = 3;
                if (shopItemType is ShopItemType.Rotation) _gm.HasRotation = true;
                if (shopItemType is ShopItemType.Water) _gm.HasWater = true;
                if (shopItemType is ShopItemType.CreditCard) _gm.HasCreditCard = true;
                SelectItem(null);
            }
            else
                _audioSource.PlayOneShot(noBuyClip);

            return _gm.Money >= price;
        }
    }
}
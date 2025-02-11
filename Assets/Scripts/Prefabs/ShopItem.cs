using Model;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prefabs
{
    public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public ShopItemType shopItemType;
        [SerializeField] private ShopMenu shopMenu;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            shopMenu.SelectItem(shopItemType);
            _rect.localScale = Vector3.one * 1.15f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            shopMenu.SelectItem(null);
            _rect.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (shopMenu.BuyItem(shopItemType))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
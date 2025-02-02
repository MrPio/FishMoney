using System;

namespace Model
{
    public enum ShopItemType
    {
        Bomb,
        Clover,
        Magnifier,
        Rotation,
        Water,
        CreditCard
    }

    public class ShopItem
    {
        public static readonly ShopItem[] ShopItems =
        {
            new(ShopItemType.Bomb,
                "Use this bomb to destroy the item you're picking up and release the hook. Useful against those worthless heavy rocks!",
                "Prefabs/Shop/bomb"),
            new(ShopItemType.Clover, "Increases your bag and treasure income. Best of luck!", "Prefabs/Shop/clover"),
            new(ShopItemType.Magnifier,
                "Sells the rocks at a higher price for archaeological purposes. Only applies to the first 3 rocks. ",
                "Prefabs/Shop/magnifier"),
            new(ShopItemType.Rotation, "Use this one to spin your hook faster!", "Prefabs/Shop/rotation"),
            new(ShopItemType.Water,
                "Gives you more power to turn the handle in the next turn. How can you resist this?",
                "Prefabs/Shop/water"),
            new(ShopItemType.CreditCard, "Sell diamonds for 50% more. Only for professionals!",
                "Prefabs/Shop/credit_card"),
        };

        public ShopItemType Type;
        public string Description, Prefab;

        public ShopItem(ShopItemType type, string description, string prefab)
        {
            Type = type;
            Description = description;
            Prefab = prefab;
        }
    }
}
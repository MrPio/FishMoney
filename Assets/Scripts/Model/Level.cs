using System.Collections.Generic;

namespace Model
{
    public class Level
    {
        public static readonly Level[] Levels =
        {
            new(1, 0, new Dictionary<ShopItemType, int> { { ShopItemType.Bomb, 345 } }),
            new(2, 3566, new Dictionary<ShopItemType, int>()),
            new(3, 3566, new Dictionary<ShopItemType, int>()),
            new(4, 3566, new Dictionary<ShopItemType, int>()),
            new(5, 3566, new Dictionary<ShopItemType, int>()),
            new(6, 3566, new Dictionary<ShopItemType, int>()),
            new(7, 3566, new Dictionary<ShopItemType, int>()),
            new(8, 3566, new Dictionary<ShopItemType, int>()),
            new(9, 3566, new Dictionary<ShopItemType, int>()),
            new(10, 3566, new Dictionary<ShopItemType, int>()),
            new(11, 3566, new Dictionary<ShopItemType, int>()),
            new(12, 3566, new Dictionary<ShopItemType, int>()),
            new(13, 3566, new Dictionary<ShopItemType, int>()),
            new(14, 3566, new Dictionary<ShopItemType, int>()),
            new(15, 3566, new Dictionary<ShopItemType, int>()),
            new(16, 3566, new Dictionary<ShopItemType, int>()),
            new(17, 3566, new Dictionary<ShopItemType, int>()),
            new(18, 3566, new Dictionary<ShopItemType, int>()),
            new(19, 3566, new Dictionary<ShopItemType, int>()),
            new(20, 3566, new Dictionary<ShopItemType, int>()),
            new(21, 3566, new Dictionary<ShopItemType, int>()),
            new(22, 3566, new Dictionary<ShopItemType, int>()),
            new(23, 3566, new Dictionary<ShopItemType, int>()),
            new(24, 3566, new Dictionary<ShopItemType, int>()),
            new(25, 3566, new Dictionary<ShopItemType, int>()),
            new(26, 3566, new Dictionary<ShopItemType, int>()),
            new(27, 3566, new Dictionary<ShopItemType, int>()),
            new(28, 3566, new Dictionary<ShopItemType, int>()),
            new(29, 3566, new Dictionary<ShopItemType, int>()),
            new(30, 3566, new Dictionary<ShopItemType, int>()),
        };

        public readonly int Id;
        public readonly int Target;
        public Dictionary<ShopItemType, int> ShopItems;

        private Level(int id, int target, Dictionary<ShopItemType, int> shopItems)
        {
            Id = id;
            Target = target;
            ShopItems = shopItems;
        }
    }
}
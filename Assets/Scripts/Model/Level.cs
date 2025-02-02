using System.Collections.Generic;

namespace Model
{
    public class Level
    {
        public static readonly Level[] Levels =
        {
            new(1, 1500, new Dictionary<ShopItemType, int> { { ShopItemType.Bomb, 345 } }),
            new(2, 3566, new Dictionary<ShopItemType, int>()),
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
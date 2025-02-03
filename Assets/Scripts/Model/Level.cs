using System.Collections.Generic;

namespace Model
{
    public class Level
    {
        // Generated with the help of FishMoneyBalancer in Python
        public static readonly Level[] Levels =
        {
            new(1, 1500, new Dictionary<ShopItemType, int>(){}),
            new(2, 3200, new Dictionary<ShopItemType, int>(){{ShopItemType.Clover, 275}}),
            new(3, 5000, new Dictionary<ShopItemType, int>(){{ShopItemType.Clover, 185}, {ShopItemType.Magnifier, 420}, {ShopItemType.Bomb, 350}}),
            new(4, 7000, new Dictionary<ShopItemType, int>(){{ShopItemType.Clover, 378}, {ShopItemType.Rotation, 156}, {ShopItemType.Magnifier, 385}}),
            new(5, 9800, new Dictionary<ShopItemType, int>(){{ShopItemType.Water, 360}}),
            new(6, 12200, new Dictionary<ShopItemType, int>(){{ShopItemType.Bomb, 378}, {ShopItemType.Rotation, 108}}),
            new(7, 14318, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 518}, {ShopItemType.Clover, 154}, {ShopItemType.Water, 444}}),
            new(8, 16386, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 348}, {ShopItemType.Clover, 412}, {ShopItemType.Water, 387}, {ShopItemType.Rotation, 230}}),
            new(9, 19393, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 368}, {ShopItemType.Bomb, 250}}),
            new(10, 22701, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 877}, {ShopItemType.Rotation, 225}}),
            new(11, 27145, new Dictionary<ShopItemType, int>(){}),
            new(12, 31189, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 648}, {ShopItemType.Water, 385}, {ShopItemType.Bomb, 295}, {ShopItemType.Clover, 340}}),
            new(13, 35114, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 575}, {ShopItemType.CreditCard, 781}, {ShopItemType.Rotation, 226}}),
            new(14, 40167, new Dictionary<ShopItemType, int>(){{ShopItemType.Water, 357}, {ShopItemType.CreditCard, 999}, {ShopItemType.Magnifier, 650}}),
            new(15, 45582, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 550}, {ShopItemType.Bomb, 365}, {ShopItemType.Water, 360}}),
            new(16, 50255, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 445}, {ShopItemType.Clover, 180}, {ShopItemType.Rotation, 200}}),
            new(17, 54789, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 485}, {ShopItemType.Clover, 260}, {ShopItemType.Rotation, 223}, {ShopItemType.Bomb, 325}}),
            new(18, 57574, new Dictionary<ShopItemType, int>(){{ShopItemType.Water, 247}}),
            new(19, 61715, new Dictionary<ShopItemType, int>(){{ShopItemType.Clover, 374}, {ShopItemType.CreditCard, 435}, {ShopItemType.Magnifier, 344}}),
            new(20, 65928, new Dictionary<ShopItemType, int>(){{ShopItemType.Rotation, 223}, {ShopItemType.Magnifier, 354}, {ShopItemType.Water, 377}, {ShopItemType.CreditCard, 789}}),
            new(21, 70589, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 848}, {ShopItemType.Water, 395}, {ShopItemType.Bomb, 295}, {ShopItemType.Clover, 340}, {ShopItemType.Rotation, 200}}),
            new(22, 75368, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 454}, {ShopItemType.CreditCard, 500}}),
            new(23, 80913, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 745}, {ShopItemType.Water, 522}, {ShopItemType.Rotation, 110}}),
            new(24, 85451, new Dictionary<ShopItemType, int>(){{ShopItemType.Clover, 240}, {ShopItemType.Magnifier, 345}, {ShopItemType.Bomb, 233}}),
            new(25, 90129, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 435}, {ShopItemType.Water, 308}, {ShopItemType.Rotation, 180}}),
            new(26, 95698, new Dictionary<ShopItemType, int>(){{ShopItemType.Magnifier, 604}, {ShopItemType.Water, 228}, {ShopItemType.CreditCard, 1135}}),
            new(27, 104486, new Dictionary<ShopItemType, int>(){{ShopItemType.Water, 348}, {ShopItemType.Clover, 260}}),
            new(28, 112357, new Dictionary<ShopItemType, int>(){{ShopItemType.CreditCard, 880}, {ShopItemType.Rotation, 188}, {ShopItemType.Bomb, 333}}),
            new(29, 119758, new Dictionary<ShopItemType, int>(){{ShopItemType.Bomb, 299}, {ShopItemType.CreditCard, 880}, {ShopItemType.Clover, 445}}),
            new(30, 130000, new Dictionary<ShopItemType, int>()),
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
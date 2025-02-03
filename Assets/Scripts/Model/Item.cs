using System;

namespace Model
{
    public enum ItemType
    {
        CoinS,
        CoinM,
        CoinL,
        RockS,
        RockL,
        Sack,
        Treasure,
        Explosive,
        Diamond,
        Bird,
        BirdDiamond,
        Bomb,
    }

    public class Item
    {
        public static readonly Item[] Items =
        {
            new(ItemType.CoinS, 5f, 200, "Audio/Items/coin"),
            new(ItemType.CoinM, 8f, 300, "Audio/Items/coin"),
            new(ItemType.CoinL, 15f, 500, "Audio/Items/coin"),
            new(ItemType.RockS, 12f, 50, "Audio/Items/rock"),
            new(ItemType.RockL, 22.5f, 100, "Audio/Items/rock"),
            new(ItemType.Sack, 7f, null, "Audio/Items/sack"),
            new(ItemType.Treasure, 18f, null, "Audio/Items/treasure"),
            new(ItemType.Explosive, 5f, 10, "Audio/Items/explosive"),
            new(ItemType.Diamond, 3.5f, 1000, "Audio/Items/diamond"),
            new(ItemType.Bird, 2.5f, 10, "Audio/Items/bird"),
            new(ItemType.BirdDiamond, 4f, 1000, "Audio/Items/bird"),
            new(ItemType.Bomb, 3f, null, "Audio/Items/coin"),
        };

        public readonly float Weight;
        private readonly int? _value;
        public readonly ItemType Type;
        public readonly string Clip;

        private Item(ItemType type, float weight, int? value, string clip)
        {
            Type = type;
            Weight = weight;
            _value = value;
            Clip = clip;
        }

        public int Value
        {
            get
            {
                var random = new Random();
                return Type switch
                {
                    ItemType.Sack => random.Next(2) == 0 ? 0 : random.Next(100, 600),
                    ItemType.Treasure => random.Next(2) == 0 ? 0 : random.Next(500, 1300),
                    _ => _value ?? 0
                };
            }
        }
    }
}
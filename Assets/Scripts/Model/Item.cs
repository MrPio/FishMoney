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
        Pouch,
        Treasure,
        Explosive,
        Diamond,
        Bird,
        BirdDiamond,
    }

    public class Item
    {
        public static readonly Item[] Items =
        {
            new(ItemType.CoinS, 5f, 200, "CoinS"),
            new(ItemType.CoinM, 8f, 300, "CoinM"),
            new(ItemType.CoinL, 15f, 500, "CoinL"),
            new(ItemType.RockS, 12f, 50, "RockS"),
            new(ItemType.RockL, 22.5f, 100, "RockL"),
            new(ItemType.Pouch, 7f, null, "Pouch"),
            new(ItemType.Treasure, 18f, null, "Treasure"),
            new(ItemType.Explosive, 5f, 10, "Explosive"),
            new(ItemType.Diamond, 3.5f, 1000, "Diamond"),
            new(ItemType.Bird, 2.5f, 10, "Bird"),
            new(ItemType.BirdDiamond, 4f, 1000, "BirdDiamond"),
        };

        public float Weight;
        private int? _value;
        public ItemType Type;
        public string Prefab;

        public Item(ItemType type, float weight, int? value, string prefab)
        {
            Type = type;
            Weight = weight;
            _value = value;
            Prefab = prefab;
        }

        public int Value
        {
            get
            {
                var random = new Random();
                return Type switch
                {
                    ItemType.Pouch => random.Next(2) == 0 ? 0 : random.Next(100, 600),
                    ItemType.Treasure => random.Next(2) == 0 ? 0 : random.Next(500, 1500),
                    _ => _value ?? 0
                };
            }
        }
    }
}
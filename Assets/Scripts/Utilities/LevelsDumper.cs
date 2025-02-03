using System;
using System.Collections.Generic;
using Managers;
using Model;
using UnityEngine;
using Level = Prefabs.Level;

namespace Utilities
{
    [Serializable]
    public class LevelDump
    {
        public int Index, CoinL, CoinM, CoinS, RockS, RockL, Sack, Treasure, Diamond;

        public LevelDump(int index)
        {
            Index = index;
        }

        public int TotalValue => CoinL * 500 + CoinM * 300 + CoinS * 200 + RockS * 50 + RockL * 100 + Sack * 300 +
                                 Treasure * 800 + Diamond * 1000;
    }

    public class LevelsDumper : MonoBehaviour
    {
        [SerializeField] private Transform levelsContainer;

        private void Start()
        {
            var levels = new List<LevelDump>();
            foreach (var level in levelsContainer.GetComponentsInChildren<Level>())
            {
                var levelDump = new LevelDump(level.LevelModel.Id);
                foreach (var item in level.GetComponentsInChildren<Prefabs.Item>())
                {
                    if (item.ItemModel.Type is ItemType.CoinS) levelDump.CoinS++;
                    if (item.ItemModel.Type is ItemType.CoinM) levelDump.CoinM++;
                    if (item.ItemModel.Type is ItemType.CoinL) levelDump.CoinL++;
                    if (item.ItemModel.Type is ItemType.RockS) levelDump.RockS++;
                    if (item.ItemModel.Type is ItemType.RockL) levelDump.RockL++;
                    if (item.ItemModel.Type is ItemType.Sack) levelDump.Sack++;
                    if (item.ItemModel.Type is ItemType.Treasure) levelDump.Treasure++;
                    if (item.ItemModel.Type is ItemType.Diamond or ItemType.BirdDiamond) levelDump.Diamond++;
                }

                JsonSerializer.Instance.Serialize(levelDump, "dumps/", $"level_{levelDump.Index}");
                levels.Add(levelDump);
                print(levelDump);
            }
            print(levels.Count);

            JsonSerializer.Instance.Serialize(levels, "dumps/", "levels_dump");
        }
    }
}
using clicker.battle;
using clicker.battle.character;
using clicker.general.ui;
using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableEnemies;
using static clicker.datatables.DataTableItems;

namespace clicker.general
{
    public class AssetsLibrary : MonoBehaviour
    {
        [Header("Prefabs")]
        public Behaviour_CoinReward CoinRewardPrefab;
        public UIElement_ItemDragIcon ItemDragIconPrefab;
        public EnemyData[] Enemies;
        [Header("Sprites")]
        public ItemSpriteData[] ItemSprites;

        private Dictionary<EnemyTypes, EnemyData> m_Enemies;
        private Dictionary<ItemTypes, ItemSpriteData> m_ItemSprites;

        void Awake()
        {
            InitEnemies();
            InitItemSprites();
        }

        void InitEnemies()
        {
            m_Enemies = new Dictionary<EnemyTypes, EnemyData>();
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (!m_Enemies.ContainsKey(Enemies[i].Type))
                    m_Enemies.Add(Enemies[i].Type, Enemies[i]);
            }
        }

        void InitItemSprites()
        {
            m_ItemSprites = new Dictionary<ItemTypes, ItemSpriteData>();
            for (int i = 0; i < ItemSprites.Length; i++)
            {
                if (!m_ItemSprites.ContainsKey(ItemSprites[i].Type))
                    m_ItemSprites.Add(ItemSprites[i].Type, ItemSprites[i]);
            }
        }


        public Enemy GetPrefab_Enemy(EnemyTypes type)
        {
            if (m_Enemies.ContainsKey(type))
                return m_Enemies[type].Prefab;

            return null;
        }

        public Sprite GetSprite_Item(ItemTypes type)
        {
            if (m_ItemSprites.ContainsKey(type))
                return m_ItemSprites[type].Sprite;

            return null;
        }


        [System.Serializable]
        public struct EnemyData
        {
            public EnemyTypes Type;
            public Enemy Prefab;
        }

        [System.Serializable]
        public struct ItemSpriteData
        {
            public ItemTypes Type;
            public Sprite Sprite;
        }
    }
}

﻿using clicker.battle.HP;
using clicker.general.ui;
using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    public partial class UIWindowsManager
    {
        [Header("Extends:")]
        public HPBarController EnemyHPBarControllerPrefab;
        public HPBarController PlayerHPBarControllerPrefab;

        [Header("Windows")]
        public UIWindow_Base UIWindow_CraftPrefab;
        public UIWindow_Base UIWindow_GameOver;
        public UIWindow_Base UIWindow_LevelFinished;
        public UIWindow_Base UIWindow_CraftTime;
        public UIWindow_Base UIWindow_AgeFinished;

        [Header(" - General")]
        [Header("UIElements")]
        public UIElement_ItemSlot UIElement_ItemSlotPrefab;
        public UIElement_AddItemSlot UIElement_AddItemSlotPrefab;
        public UIElement_LevelProgressBar UIElement_LevelProgressBarPrefab;
        public UIElement_Coins UIElement_CoinsPrefab;

        [Header(" - Craft")]
        public UIElement_CraftItem UIElement_CraftItemPrefab;
        public UIElement_CraftItem_RequireItem UIElement_CraftRequireItemPrefab;
        public UIElement_CraftItem_AbsorbItem UIElement_CraftAbsorbItemPrefab;

        [Header(" - Weapon")]
        public UIElement_WeaponSlotsController UIElement_WeaponSlotControllerPrefab;

        [Header(" - Food")]
        public UIElement_FoodSlotsController UIElement_FoodSlotControllerPrefab;

        [Header(" - Population")]
        public UIElement_PopulationProgressItem UIElement_PopulationProgressItemPrefab;
    }
}

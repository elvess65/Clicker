using clicker.battle.HP;
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

        [Header("UIElements")]
        [Header(" - Craft")]
        public UIElement_CraftItem UIElement_CraftItemPrefab;
        public UIElement_CraftItem_RequireItem UIElement_CraftRequireItemPrefab;

        [Header(" - Weapon")]
        public UIElement_WeaponSlot UIElement_WeaponSlotPrefab;
        public UIElement_WeaponSlotsController UIElement_WeaponSlotControllerPrefab;
        public UIElement_AddWeaponSlot UIElement_AddWeaponSlotPrefab;
    }
}

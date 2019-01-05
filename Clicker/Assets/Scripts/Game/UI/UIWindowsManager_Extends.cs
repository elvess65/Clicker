using clicker.general.ui;
using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    public partial class UIWindowsManager
    {
        [Header("Extends:")]
        [Header("Windows")]
        public UIWindow_Base UIWindow_CraftPrefab;
        [Header("UIElements")]
        public UIElement_CraftItem UIElement_CraftItemPrefab;
        public UIElement_CraftItem_RequireItem UIElement_CraftRequireItemPrefab;
        public UIElement_WeaponSlot UIElement_WeaponSlotPrefab;
        public UIElement_WeaponSlotsController UIElement_WeaponSlotControllerPrefab;
    }
}

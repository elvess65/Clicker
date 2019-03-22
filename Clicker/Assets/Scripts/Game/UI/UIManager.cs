using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    [RequireComponent(typeof(UIWindowsManager))]
    public class UIManager : MonoBehaviour
    {
        public System.Action<UIWindow_Base> OnShowCraftWindow;

        public UIWindowsManager WindowsManager { get; private set; }
        public UIElement_WeaponSlotsController WeaponSlotController { get; private set; }
        public UIElement_FoodSlotsController FoodSlotController { get; private set; }
        public UIElement_LevelProgressBar LevelProgressBar { get; private set; }

        [Header("Buttons")]
        public Button Button_ShowCraft;
        [Header("UI Parents")]
        public RectTransform UIParent_MiddleTop;
        public RectTransform UIParent_MiddleLeft;
        public RectTransform UIParent_MiddleRight;

        public void Init()
        {
            WindowsManager = GetComponent<UIWindowsManager>();

            Button_ShowCraft.onClick.AddListener(Button_ShowCraft_PressHandler);
        }

        public UIElement_WeaponSlotsController CreateWeaponSlots(DataTableItems.ItemTypes[] selectedWeapons, RectTransform parent, bool cacheSlots, bool allowClickable)
        {
            UIElement_WeaponSlotsController weaponSlotsController = Instantiate(WindowsManager.UIElement_WeaponSlotControllerPrefab, parent);
            weaponSlotsController.Init(selectedWeapons, allowClickable);

            if (cacheSlots)
                WeaponSlotController = weaponSlotsController;

            return weaponSlotsController;
        }

        public UIElement_FoodSlotsController CreateFoodSlots(DataTableItems.ItemTypes[] selectedFood, RectTransform parent, bool cacheSlots, bool allowClickable)
        {
            UIElement_FoodSlotsController foodSlotsController = Instantiate(WindowsManager.UIElement_FoodSlotControllerPrefab, parent);
            foodSlotsController.Init(selectedFood, allowClickable);

            if (cacheSlots)
                FoodSlotController = foodSlotsController;

            return foodSlotsController;
        }


        public UIElement_AddItemSlot CreateAddItemSlotButton(RectTransform parent, DataTableUpgrades.UpgradeTypes upgradeType)
        {
            UIElement_AddItemSlot addWeaponSlotButton = Instantiate(WindowsManager.UIElement_AddItemSlotPrefab, parent);
            addWeaponSlotButton.Init(upgradeType);

            return addWeaponSlotButton;
        }


        public void CreateLevelProgressBar(RectTransform parent, int level)
        {
            LevelProgressBar = Instantiate(WindowsManager.UIElement_LevelProgressBarPrefab, parent);
            LevelProgressBar.Init(level);
        }


        void Button_ShowCraft_PressHandler()
        {
            UIWindow_Base wnd = WindowsManager.ShowWindow(WindowsManager.UIWindow_CraftPrefab);

            OnShowCraftWindow?.Invoke(wnd);
        } 
    }
}

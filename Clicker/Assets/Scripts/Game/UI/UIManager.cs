using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    [RequireComponent(typeof(UIWindowsManager))]
    public class UIManager : MonoBehaviour
    {
        public UIWindowsManager WindowsManager { get; private set; }
        public UIElement_WeaponSlotsController WeaponSlotController { get; private set; }

        [Header("Buttons")]
        public Button Button_ShowCraft;
        [Header("UI Parents")]
        public RectTransform UIParent_MiddleLeft;

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


        void Button_ShowCraft_PressHandler()
        {
            WindowsManager.ShowWindow(WindowsManager.UIWindow_CraftPrefab);
        } 
    }
}

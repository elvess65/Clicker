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

        public UIElement_AddWeaponSlot CreateAddWeaponSlotButton(RectTransform parent)
        {
            UIElement_AddWeaponSlot addWeaponSlotButton = Instantiate(WindowsManager.UIElement_AddWeaponSlotPrefab, parent);
            addWeaponSlotButton.Init(GameManager.Instance.Manager_Battle.SelectedWeaponManager.CurAddSlot,
                                     GameManager.Instance.Manager_Battle.SelectedWeaponManager.TotalAddSlot);

            return addWeaponSlotButton;
        }


        void Button_ShowCraft_PressHandler()
        {
            UIWindow_Base wnd = WindowsManager.ShowWindow(WindowsManager.UIWindow_CraftPrefab);

            OnShowCraftWindow?.Invoke(wnd);
        } 
    }
}

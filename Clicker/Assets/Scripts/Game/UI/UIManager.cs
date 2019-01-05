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

        public void CreateWeaponSlots(DataTableItems.ItemTypes[] selectedWeapons)
        {
            WeaponSlotController = Instantiate(WindowsManager.UIElement_WeaponSlotControllerPrefab, UIParent_MiddleLeft);
            WeaponSlotController.Init(selectedWeapons);
        }


        void Button_ShowCraft_PressHandler()
        {
            WindowsManager.ShowWindow(WindowsManager.UIWindow_CraftPrefab);
        } 
    }
}

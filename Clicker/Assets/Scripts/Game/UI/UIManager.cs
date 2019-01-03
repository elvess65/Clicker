using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    [RequireComponent(typeof(UIWindowsManager))]
    public class UIManager : MonoBehaviour
    {
        public UIWindowsManager WindowsManager { get; private set; }

        [Header("Buttons")]
        public Button Button_ShowCraft;

        private void Start()
        {
            WindowsManager = GetComponent<UIWindowsManager>();

            Button_ShowCraft.onClick.AddListener(Button_ShowCraft_PressHandler);
        } 

        void Button_ShowCraft_PressHandler()
        {
            WindowsManager.ShowWindow(WindowsManager.UIWindow_CraftPrefab);
        }
    }
}

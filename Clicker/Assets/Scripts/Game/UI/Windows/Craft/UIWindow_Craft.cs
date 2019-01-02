using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_Craft : UIWindow_Base
    {
        [Header("Buttons")]
        public Button Button_TabMaterials;
        public Button Button_TabWeapons;
        [Header("Content")]
        public UITabContent Tab_Materials;
        public UITabContent Tab_Weapons;

        private void Start()
        {
            Show();
        }

        protected override void Init()
        {
            if (!m_IsInitialized)
            {
                Button_TabMaterials.onClick.AddListener(Button_TabMaterials_PressHandler);
                Button_TabWeapons.onClick.AddListener(Button_TabWeapons_PressHandler);

                Button_TabMaterials.onClick.Invoke();
            }

            base.Init();
        }

        void Button_TabMaterials_PressHandler()
        {
            Tab_Materials.InitTab();
        }

        void Button_TabWeapons_PressHandler()
        {
            Tab_Weapons.InitTab();
        }
    }
}

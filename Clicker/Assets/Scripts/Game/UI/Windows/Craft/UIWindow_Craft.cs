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

        protected override void Init()
        {
            if (!m_IsInitialized)
            {

            }

            base.Init();
        }

        void Button_TabMaterials_PressHandler()
        {

        }

        void Button_TabWeapons_PressHandler()
        {

        }
    }
}

using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_Craft : UIWindow_CloseButton
    {
        [Header("Buttons")]
        public Button Button_TabMaterials;
        public Button Button_TabWeapons;
        [Header("Texts")]
        public Text Text_Button_TabMaterials;
        public Text Text_Button_TabWeapons;
        [Header("Tab Content")]
        public UITabContent Tab_Materials;
        public UITabContent Tab_Weapons;

        protected override void Init()
        {
            if (!m_IsInitialized)
            {
                Button_TabMaterials.onClick.AddListener(Button_TabMaterials_PressHandler);
                Button_TabWeapons.onClick.AddListener(Button_TabWeapons_PressHandler);

                Text_Button_TabMaterials.text = "Materials";
                Text_Button_TabWeapons.text = "Weapons";

                Button_TabMaterials.onClick.Invoke();
            }

            base.Init();
        }

        public override void Hide()
        {
            base.Hide();

            Tab_Materials.DisposeOnWindowClose();
            Tab_Weapons.DisposeOnWindowClose();
        }

        void Button_TabMaterials_PressHandler()
        {
            Tab_Weapons.DeactivateTabOnSelectOther();
            Tab_Materials.InitTab();
        }

        void Button_TabWeapons_PressHandler()
        {
            Tab_Materials.DeactivateTabOnSelectOther();
            Tab_Weapons.InitTab();
        }
    }
}

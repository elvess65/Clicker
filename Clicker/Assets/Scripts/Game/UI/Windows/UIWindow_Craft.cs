using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_Craft : UIWindow_CloseButton
    {
        [Header("Buttons")]
        public Button Button_TabWeapons;
        public Button Button_TabMaterials;
        public Button Button_TabAmmo;
        public Button Button_TabFood;
        public Button Button_TabPopulation;
        [Header("Texts")]
        public Text Text_Button_TabWeapons;
        public Text Text_Button_TabMaterials;
        public Text Text_Button_Ammo;
        public Text Text_Button_Food;
        public Text Text_Button_Population;
        [Header("Tab Content")]
        public UIElement_Tab_Craft[] Tabs;
        [Header("Other")]
        public RectTransform CoinsParent;

        private UIElement_Coins m_LocalCoins;

        protected override void Init()
        {
            if (!m_IsInitialized)
            {
                Button_TabWeapons.onClick.AddListener(Button_TabWeapons_PressHandler);
                Button_TabMaterials.onClick.AddListener(Button_TabMaterials_PressHandler);
                Button_TabAmmo.onClick.AddListener(Button_TabAmmo_PressHandler);
                Button_TabFood.onClick.AddListener(Button_TabFood_PressHandler);
                Button_TabPopulation.onClick.AddListener(Button_TabPopulation_PressHandler);

                Text_Main.text = "Craft";
                Text_Button_TabWeapons.text = "Weapons";
                Text_Button_TabMaterials.text = "Materials";
                Text_Button_Ammo.text = "Ammo";
                Text_Button_Food.text = "Food";
                Text_Button_Population.text = "Population";

                m_LocalCoins = GameManager.Instance.Manager_UI.CreateUI_Coins(CoinsParent, DataManager.Instance.PlayerAccount.Coins, false);
                RectTransform coinsRectTransform = m_LocalCoins.GetComponent<RectTransform>();
                coinsRectTransform.pivot = new Vector2(0.5f, 0.5f);
                coinsRectTransform.anchorMin = Vector2.zero;
                coinsRectTransform.anchorMax = Vector2.one;
                coinsRectTransform.offsetMin = Vector2.zero;
                coinsRectTransform.offsetMax = Vector2.zero;

                DataManager.Instance.PlayerAccount.OnCoinsValueChanged += m_LocalCoins.UpdateAmount;

                Button_TabWeapons.onClick.Invoke();
            }

            base.Init();
        }

        public override void Hide()
        {
            base.Hide();

            Dispose();
        }


        void InitTab(DataTableItems.ItemFilterTypes itemFilter)
        {
            foreach (UIElement_Tab_Craft tab in Tabs)
            {
                if (tab.FilterType == itemFilter)
                    tab.InitTab();
                else
                    tab.DeactivateTabOnSelectOther();
            }
        }

        void Button_TabWeapons_PressHandler()
        {
            InitTab(DataTableItems.ItemFilterTypes.Weapons);
        }

        void Button_TabMaterials_PressHandler()
        {
            InitTab(DataTableItems.ItemFilterTypes.Materials);
        }

        void Button_TabAmmo_PressHandler()
        {
            InitTab(DataTableItems.ItemFilterTypes.Ammo);
        }

        void Button_TabFood_PressHandler()
        {
            InitTab(DataTableItems.ItemFilterTypes.Food);
        }

        void Button_TabPopulation_PressHandler()
        {
            InitTab(DataTableItems.ItemFilterTypes.Population);
        }

        void Dispose()
        {
            DataManager.Instance.PlayerAccount.OnCoinsValueChanged -= m_LocalCoins.UpdateAmount;

            foreach (UIElement_Tab_Craft tab in Tabs)
                tab.DisposeOnWindowClose();
        }
    }
}

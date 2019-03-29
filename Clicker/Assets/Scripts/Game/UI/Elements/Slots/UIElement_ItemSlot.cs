using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI слот предмета
    /// </summary>
    public class UIElement_ItemSlot : UIElement_ClickableItem
    {
        public System.Action<int> OnSlotPress;

        [Header("Tests")]
        public Text Text_ItemName;
        public Text Text_Amount;
        [Header("Images")]
        public Image Image_Progress;
        public Image Image_Icon;

        public int Index { get; private set; }
        public DataTableItems.ItemTypes Type { get; private set; }

        private int m_BagSize = 0;

        public void Init(DataTableItems.ItemTypes type, int index, int amount, float progress, int bagSize)
        {
            Index = index;

            SetItem(type, amount, progress, bagSize);

            base.Init();
        }


        /// <summary>
        /// Вывести информацию о предмете
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <param name="amount">Количество предмета</param>
        /// <param name="progress">Прогресс предмета</param>
        public void SetItem(DataTableItems.ItemTypes type, int amount, float progress, int bagSize)
        {
            Image_Icon.sprite = GameManager.Instance.AssetsLibrary.GetSprite_Item(type);

            if (type == DataTableItems.ItemTypes.Max)
            {
                Text_ItemName.text = string.Empty;
                SetAmount(0);
                SetSlotProgress(0);

                SelAlphaToImage(Image_Icon, 0.5f);
                return;
            }

            Text_ItemName.text = type.ToString();
            Type = type;

            CacheBagSize(bagSize);
            SetAmount(amount);
            SetSlotProgress(progress);

            if (Image_Icon.color.a < 0.9f)
                SelAlphaToImage(Image_Icon, 1f);
        }

        /// <summary>
        /// Отобразить прогресс предмета
        /// </summary>
        /// <param name="progress">Прогресс</param>
        public void SetSlotProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }

        /// <summary>
        /// Отобразить количество предмета
        /// </summary>
        /// <param name="amount">Количество предмета</param>
        public void SetAmount(int amount)
        {
            Text_Amount.text = amount != 0 ? string.Format("{0}/{1}", amount, m_BagSize) : string.Empty;
        }

        /// <summary>
        /// Кешировать размер сумки для последующего вывода
        /// </summary>
        public void CacheBagSize(int barSize)
        {
            m_BagSize = barSize;
        }


        protected override void Button_PressHandler()
        {
            base.Button_PressHandler();

            OnSlotPress?.Invoke(Index);
        }


        void SelAlphaToImage(Image image, float alphaValue)
        {
            Color color = Image_Icon.color;
            color.a = alphaValue;
            Image_Icon.color = color;
        }
    }
}

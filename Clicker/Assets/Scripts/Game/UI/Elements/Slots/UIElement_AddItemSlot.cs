using clicker.datatables;
using FrameworkPackage.UI.Windows;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// Кнопка, позволяющая добавлять слоты панелям 
    /// Выводит количество необходимых нажатий для добавления слота
    /// </summary>
    public class UIElement_AddItemSlot : UIElement_ClickableItem
    {
        public Text Text_Title;
        public Text Text_Progress;
        public Image Image_Progress;

        public void Init(DataTableUpgrades.UpgradeTypes upgradeType)
        {
            Text_Title.text = "Add Slot";

            UpdateProgress(upgradeType);

            base.Init();
        }

        public void UpdateProgress(DataTableUpgrades.UpgradeTypes upgradeType)
        {
            int upgradeLvl = DataManager.Instance.PlayerAccount.Upgrades.GetUpgradeLevel(upgradeType);
            int curProgress = DataManager.Instance.PlayerAccount.Upgrades.GetUpgradeProgress(upgradeType);
            int totalStep = DataTableUpgrades.GetStepsToNext(upgradeType, upgradeLvl);

            UpdateProgress(curProgress, totalStep);
        }


        void UpdateProgress(int curProgress, int totalStep)
        {
            Text_Progress.text = string.Format("{0}/{1}", curProgress, totalStep);
            Image_Progress.fillAmount = curProgress / (float)totalStep;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    public class UIElement_Workers : MonoBehaviour
    {
        [Header("Buttons")]
        public Button Button_AddWorker;
        public Button Button_UpgradeWorker;
        [Header("Texts")]
        public Text Text_WorkersState;
        public Text Text_AddWorkerPrice;
        public Text Text_UpgradeWorkerPrice;
        public Text Text_WorkersSpeed;
        public Text Text_WorkersLvl;
        [Header("Other")]
        public GameObject AddWorkerParent;
        public GameObject UpgradeWorkerParent;
        public GameObject ButtonsParent;

        private Color m_InitColorAddWorkerPrice;
        private Color m_InitColorUpgradeWorkerPrice;

        public void Init()
        {
            UpdateState();

            Button_AddWorker.onClick.AddListener(Button_AddWorker_PressHandler);
            Button_UpgradeWorker.onClick.AddListener(Button_UpgradeWorker_PressHandler);
        }

        public void UpdateState()
        {
            //Общее состояние
            int occuipitedWorkers = GameManager.Instance.AutoCraftItemsController.OccupitedWorkers;
            int workersAmount = DataManager.Instance.PlayerAccount.WorkersAmount;
            int workersLvl = DataManager.Instance.PlayerAccount.WorkersLvl;
            bool canAddWorker = GameManager.Instance.AutoCraftItemsController.CanAddWorker();
            bool canUpgradeWorker = GameManager.Instance.AutoCraftItemsController.CanUpgradeWorker();

            //Значения
            int addWorkerPrice = datatables.DataTableWorkers.GetPriceForBuy(DataManager.Instance.PlayerAccount.Age, workersAmount);
            int upgradeWorkerPrice = datatables.DataTableWorkers.GetPriceForUpgrade(DataManager.Instance.PlayerAccount.Age, DataManager.Instance.PlayerAccount.WorkersLvl);
            float tickPeriod = GameManager.Instance.AutoCraftItemsController.TickPeriod;

            //Активные объекты
            ButtonsParent.gameObject.SetActive(!(!canAddWorker && !canUpgradeWorker));
            AddWorkerParent.gameObject.SetActive(canAddWorker);
            UpgradeWorkerParent.gameObject.SetActive(canUpgradeWorker);

            //UI
            UpdateUI_WorkersState(occuipitedWorkers, workersAmount, !canAddWorker);
            UpdateUI_WorkresSpeed(tickPeriod);
            UpdateUI_WorkersLvl(workersLvl, !canUpgradeWorker);
            UpdateUI_AddWorkerPrice(addWorkerPrice);
            UpdateUI_UpgradeWorkerPrice(upgradeWorkerPrice);
        }


        void UpdateUI_WorkersState(int occuipitedWorkers, int workersAmount, bool maxWorkers)
        {
            Text_WorkersState.text = workersAmount > 0 ? string.Format("Workers: {0}/{1} {2}", occuipitedWorkers, workersAmount, maxWorkers ? "(Max)" : string.Empty) :
                                                         "No workers";

            Text_WorkersSpeed.gameObject.SetActive(workersAmount != 0);
            Text_WorkersLvl.gameObject.SetActive(workersAmount != 0);
        }

        void UpdateUI_WorkresSpeed(float speed)
        {
            Text_WorkersSpeed.text = string.Format("Tick: {0:0.00}s.", speed);
        }

        void UpdateUI_WorkersLvl(int lvl, bool maxLvl)
        {
            Text_WorkersLvl.text = string.Format("Lvl: {0} {1}", lvl, maxLvl ? "(Max)" : string.Empty);
        }

        void UpdateUI_AddWorkerPrice(int price)
        {
            if (m_InitColorAddWorkerPrice.a < 0.3f)
                m_InitColorAddWorkerPrice = Text_AddWorkerPrice.color;

            Text_AddWorkerPrice.text = price.ToString();

            Colorize(price, m_InitColorAddWorkerPrice, Text_AddWorkerPrice);
        }

        void UpdateUI_UpgradeWorkerPrice(int price)
        {
            if (m_InitColorUpgradeWorkerPrice.a < 0.3f)
                m_InitColorUpgradeWorkerPrice = Text_UpgradeWorkerPrice.color;

            Text_UpgradeWorkerPrice.text = price.ToString();

            Colorize(price, m_InitColorUpgradeWorkerPrice, Text_UpgradeWorkerPrice);
        }


        void Button_AddWorker_PressHandler()
        {
            GameManager.Instance.AutoCraftItemsController.AddWorker();
            UpdateState();
        }

        void Button_UpgradeWorker_PressHandler()
        {
            GameManager.Instance.AutoCraftItemsController.UpgradeWorker();
            UpdateState();
        }


        void Colorize(int price, Color initColor, Text text)
        {
            text.color = DataManager.Instance.PlayerAccount.Coins < price ? Color.red : initColor;
        }
    }
}
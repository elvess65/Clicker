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
            int workersAmount = GameManager.Instance.AutoCraftItemsController.WorkersAmount;
            bool canAddWorker = GameManager.Instance.AutoCraftItemsController.CanAddWorker();
            bool canUpgradeWorker = GameManager.Instance.AutoCraftItemsController.CanUpgradeWorker();

            //TODO 
            //Get cost

            //Значения
            int addWorkerPrice = 100;
            int upgradeWorkerPrice = 10;
            float tickPeriod = GameManager.Instance.AutoCraftItemsController.TickPeriod;
            int lvl = GameManager.Instance.AutoCraftItemsController.WorkersLvl;

            //Активные объекты
            ButtonsParent.gameObject.SetActive(!(!canAddWorker && !canUpgradeWorker));
            AddWorkerParent.gameObject.SetActive(canAddWorker);
            UpgradeWorkerParent.gameObject.SetActive(canUpgradeWorker);

            //UI
            UpdateUI_WorkersState(occuipitedWorkers, workersAmount, !canAddWorker);
            UpdateUI_WorkresSpeed(tickPeriod);
            UpdateUI_WorkersLvl(lvl, !canUpgradeWorker);
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
            //TODO 
            //Colorize

            Text_AddWorkerPrice.text = price.ToString();
        }

        void UpdateUI_UpgradeWorkerPrice(int price)
        {
            //TODO 
            //Colorize

            Text_UpgradeWorkerPrice.text = price.ToString();
        }


        void Button_AddWorker_PressHandler()
        {
            Debug.Log("Add worker");

            GameManager.Instance.AutoCraftItemsController.AddWorker();
            UpdateState();
        }

        void Button_UpgradeWorker_PressHandler()
        {
            Debug.Log("Upgrade worker");

            GameManager.Instance.AutoCraftItemsController.UpgradeWorker();
            UpdateState();
        }
    }
}
using clicker.battle;
using clicker.general;
using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.items
{
    /// <summary>
    /// Отвечает за автоматическое добываение предметов
    /// </summary>
    public class AutoCraftController : MonoBehaviour
    {
        public System.Action<ItemTypes, float> OnProgress;
        public System.Action<ItemTypes> OnPeriodFinished_Success;
        public System.Action<ItemTypes> OnPeriodFinished_Error;

        private int m_MaxPossibleLvl;
        private int m_MaxPossibleWorkers;
        private Dictionary<ItemTypes, PeriodicManager> m_ProcessedItems;

        public float TickPeriod { get; private set; }
        public int OccupitedWorkers => m_ProcessedItems.Count;


        public void Init(int maxPossibleWorkers, int maxPossibleLvl)
        {
            m_ProcessedItems = new Dictionary<ItemTypes, PeriodicManager>();
            m_MaxPossibleWorkers = maxPossibleWorkers;
            m_MaxPossibleLvl = maxPossibleLvl;

            SetTickPeriod();
        }

        public bool AddItemToProcessing(ItemTypes itemType)
        {
            PeriodicManager period = null;
            if (!m_ProcessedItems.ContainsKey(itemType))
            {
                if (m_ProcessedItems.Count == DataManager.Instance.PlayerAccount.WorkersAmount)
                {
                    Debug.LogError("CANNT PROCESS ITEM. MAX ITEMS PROCESSED");
                    return false;
                }

                //Создать и инициализировать период
                period = gameObject.AddComponent<PeriodicManager>();
                period.Init(TickPeriod, true);

                //Прогресс периода
                period.OnProgress += (float progress) => { OnProgress?.Invoke(itemType, progress); };

                //Окончание периода
                period.OnPeriodFinished += () => 
                {
                    //Если можно скрафтить предмет
                    if (DataManager.Instance.PlayerAccount.Inventory.CanCraftItem(itemType))
                    {
                        //Добавить тик
                        GameManager.Instance.CraftItemFactory.AddTickToItem(itemType);

                        //Вызвать соответствующее событие
                        OnPeriodFinished_Success?.Invoke(itemType);
                    }
                    else //Если нельзя скрафтить предмет - вызвать соответствующее событие
                        OnPeriodFinished_Error?.Invoke(itemType);
                };

                //Добавить в список
                m_ProcessedItems.Add(itemType, period);
            }
            else
                period = m_ProcessedItems[itemType];

            period.StartPeriod();

            return true;
        }

        public void RemoveItemFromProcessing(ItemTypes itemType)
        {
            if (m_ProcessedItems.ContainsKey(itemType))
            {
                //Получить и остановить период
                PeriodicManager period = m_ProcessedItems[itemType];
                period.StopPeriod();

                //Удалить период и списка
                m_ProcessedItems.Remove(itemType);

                //Удалить период
                Destroy(period);
            }
        }

        public float GetProcessedItemProgress(ItemTypes itemType)
        {
            if (ItemIsProcessing(itemType))
                return m_ProcessedItems[itemType].Progress;

            return 0;
        }

        public bool ItemIsProcessing(ItemTypes itemType) => m_ProcessedItems.ContainsKey(itemType);


        public void AddWorker()
        {
            if (!CanAddWorker())
                return;

            int price = datatables.DataTableWorkers.GetPriceForBuy(DataManager.Instance.PlayerAccount.Age, DataManager.Instance.PlayerAccount.WorkersAmount);
            if (DataManager.Instance.PlayerAccount.Coins < price)
            {
                Debug.LogError("Not enough resources to add worker");
                return;
            }

            DataManager.Instance.PlayerAccount.AddWorker();
            DataManager.Instance.PlayerAccount.DecrementCoins(price);
        }

        public void UpgradeWorker()
        {
            if (!CanUpgradeWorker())
                return;

            int price = datatables.DataTableWorkers.GetPriceForUpgrade(DataManager.Instance.PlayerAccount.Age, DataManager.Instance.PlayerAccount.WorkersLvl);
            if (DataManager.Instance.PlayerAccount.Coins < price)
            {
                Debug.LogError("Not enough resources to upgrade worker");
                return;
            }

            DataManager.Instance.PlayerAccount.UpgradeWorker();
            DataManager.Instance.PlayerAccount.DecrementCoins(price);

            SetTickPeriod();

            foreach (PeriodicManager periodic in m_ProcessedItems.Values)
                periodic.SetPeriod(TickPeriod);
        }

        public bool CanAddWorker() => DataManager.Instance.PlayerAccount.WorkersAmount < m_MaxPossibleWorkers;

        public bool CanUpgradeWorker() =>DataManager.Instance.PlayerAccount.WorkersAmount > 0 && DataManager.Instance.PlayerAccount.WorkersLvl < m_MaxPossibleLvl;


        void SetTickPeriod()
        {
            TickPeriod = datatables.DataTableWorkers.GetTickPeriodForLvl(DataManager.Instance.PlayerAccount.Age, DataManager.Instance.PlayerAccount.WorkersLvl);
        }
    }
}

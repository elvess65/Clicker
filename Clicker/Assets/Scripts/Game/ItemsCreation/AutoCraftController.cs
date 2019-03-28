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

        public int WorkersAmount { get; private set; }
        public float TickPeriod { get; private set; }
        public int WorkersLvl { get; private set; }
        public int OccupitedWorkers => m_ProcessedItems.Count;


        public void Init(int workersAmount, int maxPossibleWorkers, int lvl, int maxPossibleLvl)
        {
            m_ProcessedItems = new Dictionary<ItemTypes, PeriodicManager>();
            m_MaxPossibleWorkers = maxPossibleWorkers;
            WorkersLvl = lvl;
            m_MaxPossibleLvl = maxPossibleLvl;

            WorkersAmount = workersAmount;

            SetTickPeriod();
        }

        public bool AddItemToProcessing(ItemTypes itemType)
        {
            PeriodicManager period = null;
            if (!m_ProcessedItems.ContainsKey(itemType))
            {
                if (m_ProcessedItems.Count == WorkersAmount)
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
            //TODO 
            //Get cost
            //Compare cost with gold

            if (!CanAddWorker())
                return;

            WorkersAmount = Mathf.Clamp(WorkersAmount + 1, 0, m_MaxPossibleWorkers);
        }

        public void UpgradeWorker()
        {
            //TODO 
            //Get cost
            //Compare cost with gold

            if (!CanUpgradeWorker())
                return;

            WorkersLvl = Mathf.Clamp(WorkersLvl + 1, 0, m_MaxPossibleWorkers);
            SetTickPeriod();

            foreach (PeriodicManager periodic in m_ProcessedItems.Values)
                periodic.SetPeriod(TickPeriod);
        }

        public bool CanAddWorker()
        {
            return WorkersAmount < m_MaxPossibleWorkers;
        }

        public bool CanUpgradeWorker()
        {
            return WorkersAmount > 0 && WorkersLvl < m_MaxPossibleLvl;
        }


        void SetTickPeriod()
        {
            TickPeriod = datatables.DataTableWorkers.GetTickPeriodForLvl(DataManager.Instance.PlayerAccount.Age, WorkersLvl);
        }
    }
}

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

        private int m_MaxWorkers;
        private float m_TickPeriod;
        private Dictionary<ItemTypes, PeriodicManager> m_ProcessedItems;

        public void Init(int maxWorkers, float tickPeriod)
        {
            m_ProcessedItems = new Dictionary<ItemTypes, PeriodicManager>();

            //TODO
            //Load items from save

            m_MaxWorkers = maxWorkers;
            m_TickPeriod = tickPeriod;
        }

        public bool AddItemToProcessing(ItemTypes itemType)
        {
            PeriodicManager period = null;
            if (!m_ProcessedItems.ContainsKey(itemType))
            {
                if (m_ProcessedItems.Count == m_MaxWorkers)
                {
                    Debug.LogError("CANNT PROCESS ITEM. MAX ITEMS PROCESSED");
                    return false;
                }

                //Создать и инициализировать период
                period = gameObject.AddComponent<PeriodicManager>();
                period.Init(m_TickPeriod, true);

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
                        OnPeriodFinished_Error.Invoke(itemType);
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

        public bool ItemIsProcessed(ItemTypes itemType) => m_ProcessedItems.ContainsKey(itemType);
    }
}

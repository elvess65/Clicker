using clicker.battle;
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

        public void AddItemToProcessing(ItemTypes itemType)
        {
            if (!m_ProcessedItems.ContainsKey(itemType))
            {
                //TODO
                //Create periodic
                //Add item
                //Start period
            }
        }

        public void RemoveItemFromProcessing(ItemTypes itemType)
        {
            if (m_ProcessedItems.ContainsKey(itemType))
            {
                //TODO
                //Stop period
                //Remove periodic
                //Remove item
            }
        }

        public bool ItemIsProcessed(ItemTypes itemType) => m_ProcessedItems.ContainsKey(itemType);
    }
}

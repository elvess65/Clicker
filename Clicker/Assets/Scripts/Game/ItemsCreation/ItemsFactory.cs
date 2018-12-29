using clicker.datatables;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.items
{
    public class ItemsFactory : MonoBehaviour
    {
        public System.Action<ItemTypes, float> OnTickToItemAdded;

        private const int m_TICK_STEP = 1;

        private Dictionary<ItemTypes, ItemCreationData> m_ProcessedItems;   

        public void AddTickToItem(ItemTypes type)
        {
            if (m_ProcessedItems == null)
                m_ProcessedItems = new Dictionary<ItemTypes, ItemCreationData>();

            if (!m_ProcessedItems.ContainsKey(type))
            {
                ItemCreationData itemCreateData = new ItemCreationData(type, DataTableItems.GetIemDataByType(type).TicksToCreate);
                m_ProcessedItems.Add(type, itemCreateData);
            }

            //Добавить тики к предмету
            ItemCreationData curItemCreationData = m_ProcessedItems[type];
            var resultData = curItemCreationData.AddTickToItem(m_TICK_STEP);

            //Если предмет был создан - обнулить текущее количество тиков
            if (resultData.isCreated)
                curItemCreationData.ResetTicks();

            OnTickToItemAdded?.Invoke(type, resultData.progress);

            Debug.Log(m_ProcessedItems[type].ToString());  
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AddTickToItem(ItemTypes.Stick);
        }

        /// <summary>
        /// Данные о предмете, который создаеться
        /// </summary>
        private class ItemCreationData
        {
            public ItemTypes Type;      //Тип предмета
            public int TicksToCreate;   //Тики для создания

            private int m_CurTicks;     //Текущее количество тиков

            public ItemCreationData(ItemTypes type, int ticksToCreate)
            {
                Type = type;
                TicksToCreate = ticksToCreate;
                m_CurTicks = 0;
            }

            /// <summary>
            /// Добавить тики к предмету
            /// </summary>
            /// <param name="ticks">Количество тиков</param>
            /// <returns>isCreated - true если предмет был создан. progress - прогресс создания предмета</returns>
            public (bool isCreated, float progress) AddTickToItem(int ticks)
            {
                (bool isCreated, float progress) result = (false, 0);

                m_CurTicks += ticks;
                if (m_CurTicks >= TicksToCreate)
                {
                    result.isCreated = true;
                    result.progress = 1;
                }
                else
                    result.progress = m_CurTicks / (float)TicksToCreate;
                
                return result;
            }

            /// <summary>
            /// Обнулить текущее состояние тиков 
            /// </summary>
            public void ResetTicks()
            {
                m_CurTicks = 0;
            }

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(50);
                strBuilder.AppendFormat("Create item {0}. Cur Ticks: {1}. Ticks: {2}. Progress: {3}",
                    Type, m_CurTicks, TicksToCreate, (m_CurTicks / (float)TicksToCreate));

                return strBuilder.ToString();
            }
        }
    }
}

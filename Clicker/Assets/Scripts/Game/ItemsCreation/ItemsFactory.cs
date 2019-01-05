using clicker.datatables;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.items
{
    /// <summary>
    /// Отвечает за крафт предметов
    /// </summary>
    public class ItemsFactory : MonoBehaviour
    {
        public System.Action<ItemTypes, float> OnTickToItemAdded;
        public System.Action<ItemTypes> OnItemCrafted;

        private const int m_TICK_STEP = 1;

        private Dictionary<ItemTypes, ItemCreationData> m_ProcessedItems;   

        /// <summary>
        /// Добавить тики указанному предмету
        /// </summary>
        /// <param name="type">Тип предмета, которому нужно добавить тики</param>
        public void AddTickToItem(ItemTypes type)
        {
            //Создать словарь если еще не существует
            if (m_ProcessedItems == null)
                m_ProcessedItems = new Dictionary<ItemTypes, ItemCreationData>();

            //Если предмета еще нет в списке обрадатываемых - добавить
            if (!m_ProcessedItems.ContainsKey(type))
            {
                ItemCreationData itemCreateData = new ItemCreationData(type, DataTableItems.GetIemDataByType(type).TicksToCreate);
                m_ProcessedItems.Add(type, itemCreateData);
            }

            //Добавить тики к предмету
            ItemCreationData curItemCreationData = m_ProcessedItems[type];
            var resultData = curItemCreationData.AddTickToItem(m_TICK_STEP);

            //Если предмет был создан 
            if (resultData.isCreated)
            {
                //Обнулить текущее количество тиков (даже есть текущий шаг тиков превышает количество тиков для создания - за раз можно создать только один предмет)
                curItemCreationData.ResetTicks();

                //Вызов события добавления предмета
                OnItemCrafted?.Invoke(type);
            }
            else
            {
                //Вызов события добавления тиков
                OnTickToItemAdded?.Invoke(type, resultData.progress);
            }

            //Debug.Log(m_ProcessedItems[type].ToString());  
        }

        /// <summary>
        /// Получить текущий прогресс крафта указанного предмета
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <returns>Прогресс</returns>
        public float GetProgressForItem(ItemTypes type)
        {
            if (m_ProcessedItems != null && m_ProcessedItems.ContainsKey(type))
                return m_ProcessedItems[type].CurProgress;

            return 0;
        }


        /// <summary>
        /// Данные о предмете, который создаеться
        /// </summary>
        private class ItemCreationData
        {
            public ItemTypes Type;      //Тип предмета
            public int TicksToCreate;   //Тики для создания

            private int m_CurTicks;     //Текущее количество тиков

            public float CurProgress => Mathf.Clamp(m_CurTicks / (float)TicksToCreate, 0, 1); 

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
                    m_CurTicks = TicksToCreate;
                    result.isCreated = true;
                }

                result.progress = CurProgress;
                
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
                                        Type, m_CurTicks, TicksToCreate, CurProgress);

                return strBuilder.ToString();
            }
        }
    }
}

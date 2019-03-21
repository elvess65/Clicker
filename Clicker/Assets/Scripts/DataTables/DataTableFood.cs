using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    public class DataTableFood
    {
        private static Dictionary<ItemTypes, Food> m_Food;

        public static void SetData(FoodData[] data)
        {
            if (data == null)
                return;

            m_Food = new Dictionary<ItemTypes, Food>();

            for (int i = 0; i < data.Length; i++)
            {
                //Создать предмет
                Food item = new Food(data[i].Type, data[i].RestoreHP);

                m_Food.Add(data[i].Type, item);
            }
        }

        /// <summary>
        /// Получить данные о еде по типу
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <returns>Данные о предмете</returns>
        public static Food GetFoodDataByType(ItemTypes type)
        {
            if (m_Food.ContainsKey(type))
                return m_Food[type];

            return null;
        }

        #region Data Structures
        /// <summary>
        /// Представляет описание еды
        /// </summary>
        public class Food
        {
            private DataTableItems.ItemTypes m_Type;
            private int m_RestoreHP;

            public DataTableItems.ItemTypes Type => m_Type;
            public int RestoreHP => m_RestoreHP;

            public Food(DataTableItems.ItemTypes type, int restoreHP)
            {
                m_Type = type;
                m_RestoreHP = restoreHP;
            }

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(50);

                //Вывести общие данные 
                strBuilder.AppendFormat("Food: {0}. Restore HP: {1}", m_Type, m_RestoreHP);

                return strBuilder.ToString();
            }
        }
        #endregion
    }
}

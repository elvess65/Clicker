using System.Collections.Generic;
using System.Text;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    public class DataTablePeriodic
    {
        private static Dictionary<ItemTypes, Periodic> m_Periodic;

        public static void SetData(PeriodicData[] data)
        {
            if (data == null)
                return;

            m_Periodic = new Dictionary<ItemTypes, Periodic>();

            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    //Создать предмет
                    Periodic periodic = new Periodic(data[i].Type, data[i].PopulationMultiplayer);

                    m_Periodic.Add(data[i].Type, periodic);
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Получить данные о периоде по типу
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <returns>Данные о периоде</returns>
        public static Periodic GetPeriodicDataByItemID(ItemTypes type)
        {
            if (m_Periodic.ContainsKey(type))
                return m_Periodic[type];

            return null;
        }


        public class Periodic
        {
            private ItemTypes m_Type;
            private float m_PopulationMultiplayer;

            public ItemTypes Type => m_Type;
            public float PopulationMultiplayer => m_PopulationMultiplayer;

            public Periodic(ItemTypes type, float populationMultiplayer)
            {
                m_Type = type;
                m_PopulationMultiplayer = populationMultiplayer;
            }

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(50);

                //Вывести общие данные 
                strBuilder.AppendFormat("Population: {0}. Multuplayer: {1}", Type, PopulationMultiplayer);

                return strBuilder.ToString();
            }
        }
    }
}

using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    interface iPeriodicDataLoader
    {
        PeriodicData[] GetData(int dataParam);
    }

    public class PeriodicDataLoader_Local : iPeriodicDataLoader
    {
        public PeriodicData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalPeriodicDataEditor>().Data_Periodic;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct PeriodicData
    {
        public DataTableItems.ItemTypes Type;
        public float PopulationMultiplayer;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);

            //Вывести общие данные 
            strBuilder.AppendFormat("Population: {0}. Multuplayer: {1}", Type, PopulationMultiplayer);

            return strBuilder.ToString();
        }
    }
}

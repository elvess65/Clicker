using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    interface iFoodDataLoader
    {
        FoodData[] GetData(int dataParam);
    }

    public class FoodDataLoader_Local : iFoodDataLoader
    {
        public FoodData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalFoodDataEditor>().Data_Food;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct FoodData
    {
        public DataTableItems.ItemTypes Type;
        public int RestoreHP;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);

            //Вывести общие данные 
            strBuilder.AppendFormat("Food: {0}. Restore HP: {1}", Type, RestoreHP);

            return strBuilder.ToString();
        }
    }
}

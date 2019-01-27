using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    interface iWeaponsDataLoader
    {
        WeaponsData[] GetData(int dataParam);
    }

    public class WeaponsDataLoader_Local : iWeaponsDataLoader
    {
        public WeaponsData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalWeaponsDataEditor>().Data_Weapons;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct WeaponsData
    {
        public DataTableItems.ItemTypes Type;
        public int Damage;
        public int Durability;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);

            //Вывести общие данные 
            strBuilder.AppendFormat("Weapon: {0}. Damage: {1}. Durability: {2}", Type, Damage, Durability);

            return strBuilder.ToString();
        }
    }
}

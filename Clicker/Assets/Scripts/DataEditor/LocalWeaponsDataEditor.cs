using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    public class LocalWeaponsDataEditor : MonoBehaviour
    {
        public WeaponsData[] Data_Weapons;

        /// <summary>
        /// Симуляция пакета данных из базы
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
}

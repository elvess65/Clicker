using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    interface iItemsDataLoader
    {
        ItemsData[] GetData(int dataParam);
        ItemsCraftIgnoreData[] GetIgnoreData(int dataParam);
    }

    public class ItemsDataLoader_Local : iItemsDataLoader
    {
        public ItemsData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalItemsDataEditor>().Data_Items;
        }

        public ItemsCraftIgnoreData[] GetIgnoreData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalItemsDataEditor>().Data_CraftIgnoreItems;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct ItemsData
    {
        public ItemTypes Type;
        public int TickToCreate;
        public bool AllowAutocraft;
        public List<ItemFilterTypes> FilterTypes;
        public ItemDataAmountContainer[] RequiredItems;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(RequiredItems.Length * 10 + 20);

            //Вывести общие данные о предмете
            strBuilder.AppendFormat("Item: {0}. Ticks: {1}", Type, TickToCreate);

            //Вывести необходимые для создания предмета предметы
            if (RequiredItems.Length > 0)
            {
                strBuilder.AppendFormat("\nRequired Items {0}:\n", RequiredItems.Length);
                for (int i = 0; i < RequiredItems.Length; i++)
                    strBuilder.AppendFormat(" - {0}\n", RequiredItems[i].ToString());
            }

            return strBuilder.ToString();
        }

        [System.Serializable]
        public struct ItemDataAmountContainer
        {
            public ItemTypes Type;
            public int Amount;

            public override string ToString()
            {
                return string.Format("Type: {0}. Amount: {1}", Type, Amount);
            }
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct ItemsCraftIgnoreData
    {
        public ItemTypes CraftItem;
        public ItemTypes[] IgnoreItems;
    }
}

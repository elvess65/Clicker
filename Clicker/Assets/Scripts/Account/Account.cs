using clicker.datatables;
using System.Collections.Generic;
using System.Text;
using static clicker.datatables.DataTableItems;

namespace clicker.account
{
    /// <summary>
    /// Представляет все данные игрока
    /// </summary>
    public class Account
    {
        public AccountInventory Inventory;

        public Account()
        {
            Inventory = new AccountInventory();
        }

        /// <summary>
        /// Представялет инвентарь игрока
        /// </summary>
        public class AccountInventory
        {
            private Dictionary<ItemTypes, ItemAmountContainer> m_Items;

            public AccountInventory()
            {
                m_Items = new Dictionary<ItemTypes, ItemAmountContainer>();

                //Добавить предмет по-умолчанию
                m_Items.Add(ItemTypes.Hand, new ItemAmountContainer(ItemTypes.Hand));
            }

            /// <summary>
            /// Получить количество определенного предмета
            /// </summary>
            public int GetItemAmount(ItemTypes type)
            {
                if (m_Items.ContainsKey(type))
                    return m_Items[type].Amount;

                return 0;
            }

            /// <summary>
            /// Есть ли достаточное количество указанного предмета
            /// </summary>
            /// <param name="type">Тип предмета</param>
            /// <param name="amount">Запрашиваемое количество</param>
            /// <returns>true если есть достаточное количество</returns>
            public bool HasAmountOfItem(ItemTypes type, int amount)
            {
                if (m_Items.ContainsKey(type) && m_Items[type].Amount >= amount)
                    return true;

                return false;
            }

            /// <summary>
            /// Можно ли создать предмет (Хватает ли у игрока предметов, необходимых для создания предмета)
            /// </summary>
            /// <param name="type">Тип предмета</param>
            /// <returns>true если можно создать</returns>
            public bool CanCraftItem(ItemTypes type)
            {
                int hasItemCount = 0;
                DataTableItems.Item itemData = DataTableItems.GetIemDataByType(type);
                for(int i = 0; i < itemData.RequiredItems.Length; i++)
                {
                    //Если есть достаточное количество предметов указанного типа
                    if (HasAmountOfItem(itemData.RequiredItems[i].Type, itemData.RequiredItems[i].Amount))
                        hasItemCount++;
                }

                return hasItemCount == itemData.RequiredItems.Length;
            }


            /// <summary>
            /// Добавить количество определенного предмета
            /// </summary>
            public void AddItem(ItemTypes type, int amount = 1)
            {
                if (!m_Items.ContainsKey(type))
                    m_Items.Add(type, new ItemAmountContainer(type, amount));
                else 
                    m_Items[type].AddAmount(amount);
            }

            /// <summary>
            /// Отнять количество определенного предмета
            /// </summary>
            public void RemoveItem(ItemTypes type, int amount = 1)
            {
                if (m_Items.ContainsKey(type))
                {
                    m_Items[type].RemoveAmount(amount);

                    //Если больше нет этого предмета удалить его из списка
                    if (m_Items[type].Amount == 0)
                        m_Items.Remove(type);
                }
            }


            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(100);
                strBuilder.AppendFormat("Inventory: {0}", m_Items.Count);

                if (m_Items.Count > 0)
                {
                    strBuilder.Append("\n");
                    strBuilder.Append("Items:\n");
                    foreach (ItemAmountContainer itemAmountContainer in m_Items.Values)
                        strBuilder.AppendFormat(" - {0}\n", itemAmountContainer);
                }

                return strBuilder.ToString();
            }
        }
    }
}

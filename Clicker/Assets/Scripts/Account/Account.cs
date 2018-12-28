using UnityEngine;
using System.Collections.Generic;
using static clicker.datatables.DataTableItems;
using System.Text;

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
                    strBuilder.Append("Items:\n");
                    foreach (ItemAmountContainer itemAmountContainer in m_Items.Values)
                        strBuilder.AppendFormat(" - {0}\n", itemAmountContainer);
                }

                return strBuilder.ToString();
            }
        }
    }
}

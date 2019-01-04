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

            public AccountWeapons WeaponState { get; private set; }

            public AccountInventory()
            {
                WeaponState = new AccountWeapons();
                WeaponState.OnWeaponBroken += WeaponBrokenHandler;

                m_Items = new Dictionary<ItemTypes, ItemAmountContainer>();

                //Добавить предмет по-умолчанию
                AddItem(ItemTypes.Hand);
            }

            void WeaponBrokenHandler(ItemTypes type)
            {
                RemoveItem(type);
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
                {
                    m_Items.Add(type, new ItemAmountContainer(type, amount));

                    //Данные о добавленном предмете
                    DataTableItems.Item itemData = DataTableItems.GetIemDataByType(type);
                    //Если добаленный предмет - оружие
                    if (itemData.MatchFilter(ItemFilterTypes.Weapons))
                        WeaponState.AddWeapon(type);
                }
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
                    {
                        m_Items.Remove(type);

                        //Данные об удаленном предмете
                        DataTableItems.Item itemData = DataTableItems.GetIemDataByType(type);
                        //Если удаленный предмет - оружие
                        if (itemData.MatchFilter(ItemFilterTypes.Weapons))
                            WeaponState.RemoveWeapon(type, amount);
                    }
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


            /// <summary>
            /// Ведет учет стостояния оружия, которое есть у игрока. Количество оружия отслеживаеться интвентарем. 
            /// Если оружие ломаеться или пропадает происходит обнуление сотосния (достаеться новое)
            /// </summary>
            public class AccountWeapons
            {
                public System.Action<ItemTypes> OnWeaponBroken;
                public System.Action<ItemTypes, float> OnUseWeapon;

                private Dictionary<ItemTypes, WeaponStateContainer> m_Weapons;

                public AccountWeapons()
                {
                    m_Weapons = new Dictionary<ItemTypes, WeaponStateContainer>();

                    //Добавить оружие по-умолчанию
                    AddWeapon(ItemTypes.Hand);
                }

                public void AddWeapon(ItemTypes type)
                {
                    //Если оружие указанного типа еще нет
                    if (!m_Weapons.ContainsKey(type))
                        m_Weapons.Add(type, new WeaponStateContainer(DataTableWeapons.GetWeaponDataByType(type)));
                }

                public void RemoveWeapon(ItemTypes type, int amount)
                {
                    if (m_Weapons.ContainsKey(type))
                        m_Weapons.Remove(type);
                }

                /// <summary>
                /// Использовать оружие (Отнять прочность)
                /// </summary>
                /// <param name="type">Тип оружия</param>
                /// <returns>true если удалось использовать оружие</returns>
                public bool UseWeapon(ItemTypes type)
                {
                    if (m_Weapons.ContainsKey(type))
                    {
                        //Если после использования оружие осталось целым
                        if (m_Weapons[type].UseWeapon())
                            OnUseWeapon?.Invoke(type, m_Weapons[type].CurDurabilityProgress);
                        else
                            OnWeaponBroken?.Invoke(type);

                        return true;
                    }

                    return false;
                }


                public override string ToString()
                {
                    StringBuilder strBuilder = new StringBuilder(m_Weapons.Count * 5 + 20);

                    strBuilder.AppendFormat("Account weapons: {0}", m_Weapons.Count);

                    if (m_Weapons.Count > 0)
                    {
                        strBuilder.Append("\n");

                        foreach (WeaponStateContainer weaponState in m_Weapons.Values)
                            strBuilder.AppendFormat(" - {0}\n", weaponState.ToString());
                    }

                    return strBuilder.ToString();
                }

                /// <summary>
                /// Структура для сохранения данных о текущем состоянии оружия и количестве оружия этого типа
                /// </summary>
                private class WeaponStateContainer
                {
                    private ItemTypes m_Type;
                    private int m_CurDurability;
                    private int m_Durability;

                    /// <summary>
                    /// Прогресс до того как оружие сломаеться
                    /// </summary>
                    public float CurDurabilityProgress => m_CurDurability / (float)m_Durability;

                    public WeaponStateContainer(DataTableWeapons.Weapon weapon)
                    {
                        m_Type = weapon.Type;
                        m_Durability = weapon.Durability;

                        ResetDurability();
                    }

                    /// <summary>
                    /// Использовать оружие (отнять прочность)
                    /// </summary>
                    /// <returns>true если после использования оружие еще целое</returns>
                    public bool UseWeapon()
                    {
                        //Если прочность меньше 0 - оружие не может сломаться
                        if (m_Durability < 0)
                            return true;

                        m_CurDurability--;
                        if (m_CurDurability < 0)
                            m_CurDurability = 0;

                        if (m_CurDurability == 0)
                        {
                            ResetDurability();
                            return false;
                        }

                        return true;
                    }

                    /// <summary>
                    /// Вернуть данные о текущей прочности к изначальному состоянию
                    /// </summary>
                    void ResetDurability()
                    {
                        m_CurDurability = m_Durability;
                    }


                    public override string ToString()
                    {
                        StringBuilder strBuilder = new StringBuilder(50);

                        strBuilder.AppendFormat("Weapon: {0}. CurDurability: {1}. Durability: {2}", m_Type, m_CurDurability, m_Durability);

                        return strBuilder.ToString();
                    }

                }
            }
        }
    }
}

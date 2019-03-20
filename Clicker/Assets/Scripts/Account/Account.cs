using clicker.datatables;
using clicker.general;
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

        public int AccountID { get; private set; }
        public int HP { get; private set; }
        public int CraftTime { get; private set; }

        //Level
        public DataTableLevels.AgeTypes Age { get; private set; }
        public int Level { get; private set; }
        
        public Account(int accountID, int hp, int craftTime, DataTableLevels.AgeTypes age, int level, 
            DataTableItems.ItemTypes[] selectedWeapon, DataTableItems.ItemTypes[] selectedFood, Dictionary<ItemFilterTypes, int> bags)
        {
            //Base
            AccountID = accountID;
            HP = hp;
            CraftTime = craftTime;

            //Level
            Age = age;
            Level = level;

            //Other
            Inventory = new AccountInventory(selectedWeapon, selectedFood, bags);
        }

        public void IncrementAge()
        {
            Age++;
        }

        public void IncrementLevel()
        {
            Level++;
        }

        public void ResetProgress()
        {
            Age = DataTableLevels.AgeTypes.FirstAge;
            Level = 0;
        }


        /// <summary>
        /// Представялет инвентарь игрока
        /// </summary>
        public class AccountInventory
        {
            private Dictionary<ItemTypes, ItemAmountContainer> m_Items;

            public AccountWeapons WeaponState { get; private set; }
            public AccountBags BagsState { get; private set; }

            public List<DataTableItems.ItemTypes> SelectedWeapon { get; private set; }
            public List<DataTableItems.ItemTypes> SelectedFood { get; private set; }

            public const ItemTypes DEFAULT_ITEM = ItemTypes.Hand;

            public AccountInventory(DataTableItems.ItemTypes[] selectedWeapon, DataTableItems.ItemTypes[] selectedFood, Dictionary<ItemFilterTypes, int> bags)
            {
                //Состояние сумок
                BagsState = new AccountBags(bags);

                //Состояние оружия
                WeaponState = new AccountWeapons();
                WeaponState.OnWeaponBroken += WeaponBrokenHandler;

                //Выбранное оружие
                SelectedWeapon = new List<DataTableItems.ItemTypes>();
                for (int i = 0; i < selectedWeapon.Length; i++)
                    SelectedWeapon.Add(selectedWeapon[i]);

                //Выбрання еда
                SelectedFood = new List<DataTableItems.ItemTypes>();
                for (int i = 0; i < selectedFood.Length; i++)
                    SelectedFood.Add(selectedFood[i]);

                //Предметы
                m_Items = new Dictionary<ItemTypes, ItemAmountContainer>();

                //Добавить предмет по-умолчанию
                AddItem(DEFAULT_ITEM);
            }


            /// <summary>
            /// Получить количество определенного предмета
            /// </summary>
            public int GetItemAmount(ItemTypes type)
            {
                if (m_Items.ContainsKey(type) && !type.Equals(DEFAULT_ITEM))
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
            /// Есть ли хотя бы одинуказанный предмет
            /// </summary>
            /// <param name="type">Тип предмета</param>
            /// <returns>true если есть</returns>
            public bool HasItem(ItemTypes type)
            {
                return GetItemAmount(type) > 0;
            }

            /// <summary>
            /// Можно ли создать предмет (Хватает ли у игрока предметов, необходимых для создания предмета)
            /// </summary>
            /// <param name="type">Тип предмета</param>
            /// <returns>true если можно создать</returns>
            public bool CanCraftItem(ItemTypes type)
            {
                int hasItemCount = 0;
                DataTableItems.Item itemData = DataTableItems.GetItemDataByType(type);
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
                    DataTableItems.Item itemData = DataTableItems.GetItemDataByType(type);

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
                        DataTableItems.Item itemData = DataTableItems.GetItemDataByType(type);

                        //Если удаленный предмет - оружие
                        if (itemData.MatchFilter(ItemFilterTypes.Weapons))
                            WeaponState.RemoveWeapon(type, amount);
                    }
                }
            }

            /// <summary>
            /// Получить список предметов по определенному фильтру
            /// </summary>
            /// <param name="filterType">Фильтр</param>
            /// <returns>Массив предметов</returns>
            public ItemTypes[] GetItemsByFilterType(ItemFilterTypes filterType)
            {
                List<ItemTypes> result = new List<ItemTypes>();

                foreach (ItemTypes itemType in m_Items.Keys)
                {
                    if (HasItem(itemType))
                    {
                        //Данные о предмете
                        DataTableItems.Item itemData = DataTableItems.GetItemDataByType(itemType);
                        if (itemData.MatchFilter(filterType))
                            result.Add(itemType);
                    }
                }

                return result.ToArray();
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


            void WeaponBrokenHandler(ItemTypes type)
            {
                //Удалить предмет
                RemoveItem(type);

                //Если после удаления предмета еще остаются предметы, но сумка может быть опустошена
                if (HasItem(type))
                {
                    //Отнять предмет из сумки (из сумки удаляются только тогда, когда предмет закончился)
                    BagsState.RemoveItemFromBag(type, false);

                    //Если сумка была опустошена
                    if (!BagsState.HasItemInBag(type))
                        WeaponState.CallRemoveEventAndRemoveFromBag(type);
                }
            }


            /// <summary>
            /// Ведет учет стостояния оружия, которое есть у игрока. Количество оружия отслеживаеться интвентарем. 
            /// Если оружие ломаеться или пропадает происходит обнуление сотосния (достаеться новое)
            /// </summary>
            public class AccountWeapons
            {
                public System.Action<ItemTypes> OnWeaponBroken;
                public System.Action<ItemTypes, float> OnUseWeapon;
                public System.Action<ItemTypes> OnRemoveWeapon;

                private Dictionary<ItemTypes, WeaponStateContainer> m_Weapons;

                public AccountWeapons()
                {
                    m_Weapons = new Dictionary<ItemTypes, WeaponStateContainer>();

                    //Добавить оружие по-умолчанию
                    AddWeapon(DEFAULT_ITEM);
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
                    {
                        m_Weapons.Remove(type);
                        CallRemoveEventAndRemoveFromBag(type);
                    }
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

                /// <summary>
                /// Получить текущее состояние прочности оружия
                /// </summary>
                /// <param name="type">Тип оружия</param>
                /// <returns>Прогресс до поломки</returns>
                public float GetDurabilityProgress(ItemTypes type)
                {
                    if (m_Weapons.ContainsKey(type))
                        return m_Weapons[type].CurDurabilityProgress;

                    return 1;
                }

                /// <summary>
                /// Вызвать событие удаления оружия, не удаляя его
                /// </summary>
                public void CallRemoveEventAndRemoveFromBag(ItemTypes type)
                {
                    OnRemoveWeapon?.Invoke(type);
                    DataManager.Instance.PlayerAccount.Inventory.BagsState.RemoveItemFromBag(type, true);
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

            /// <summary>
            /// Сумки для вещей игрока
            /// </summary>
            public class AccountBags
            {
                private Dictionary<ItemFilterTypes, int> m_Bags;
                private Dictionary<ItemTypes, int> m_BagState;

                public AccountBags(Dictionary<ItemFilterTypes, int> bags)
                {
                    m_Bags = new Dictionary<ItemFilterTypes, int>(bags);
                    m_BagState = new Dictionary<ItemTypes, int>();
                }

                public int GetBagSize(ItemFilterTypes itemFilterType)
                {
                    if (m_Bags.ContainsKey(itemFilterType))
                        return m_Bags[itemFilterType];

                    return 0;
                }

                public int GetItemAmountInBag(ItemTypes item)
                {
                    if (m_BagState.ContainsKey(item))
                        return m_BagState[item];

                    return 0;
                }

                public bool HasItemInBag(ItemTypes item)
                {
                    return GetItemAmountInBag(item) > 0; 
                }

                public void AddItemToBag(ItemTypes item)
                {
                    int itemAmount = DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(item);
                    ItemFilterTypes itemBagType = DataTableItems.GetItemDataByType(item).SingleFilter;
                    int itemBagSize = GetBagSize(itemBagType);
                    int amountInBag = UnityEngine.Mathf.Clamp(itemAmount, 0, itemBagSize);

                    //Если в сумке еще нет предмета
                    if (!m_BagState.ContainsKey(item))
                        m_BagState.Add(item, amountInBag);
                    else
                        m_BagState[item] = amountInBag;

                    UnityEngine.Debug.Log("Add item to bag: " + GetItemAmountInBag(item) + "/" + GetBagSize(itemBagType));
                }

                public void RemoveItemFromBag(ItemTypes item, bool removeAll)
                {
                    if (m_BagState.ContainsKey(item))
                    {
                        if (removeAll)
                            m_BagState.Remove(item);
                        else
                        {
                            m_BagState[item]--;

                            if (m_BagState[item] <= 0)
                                m_BagState.Remove(item);
                        }
                    }

                    UnityEngine.Debug.Log("RemoveItemFromBag " + item);
                    if (m_BagState.ContainsKey(item))
                        UnityEngine.Debug.Log("Amount after removing: " + m_BagState[item]);
                }
            }
        }
    }
}

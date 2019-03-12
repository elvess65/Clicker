using System.Collections.Generic;
using System.Text;

namespace clicker.datatables
{
    public static class DataTableItems
    {
        private static Dictionary<ItemTypes, Item> m_Items;

        public static void SetData(ItemsData[] data)
        {
            if (data == null)
                return;

            m_Items = new Dictionary<ItemTypes, Item>();

            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    //Создать предмет
                    Item item = new Item(data[i].Type, data[i].TickToCreate, data[i].FilterTypes);

                    //Создать необходимые для создания предмета предметы
                    for (int j = 0; j < data[i].RequiredItems.Length; j++)
                        item.AddRequiredItem(data[i].RequiredItems[j].Type, data[i].RequiredItems[j].Amount);

                    m_Items.Add(data[i].Type, item);
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Получить данные о предмете по типу
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <returns>Данные о предмете</returns>
        public static Item GetItemDataByType(ItemTypes type)
        {
            if (m_Items.ContainsKey(type))
                return m_Items[type];

            return null;
        }

        #region Data Structures
        public enum ItemTypes
        {
            Hand,
            Stick,              //Палка
            Stone,              //Камень
            Spear,              //Деревяное копье
            StickWithStone,     //Палка с камнем
            Thread,             //Нить
            WoodenArrow,        //Деревяная стрела
            Bow,                //Лук
            StonePike,          //Каменный наконечник
            Arrow_StonePike,    //Стрела с каменным наконечником
            Spear_StonePike,    //Копье с каменным наконечником
            Fire,               //Огонь
            Berries,            //Ягоды
            Meat,               //Мясо
            GrilledMeat,        //Жаренное мясо
            Water,              //Вода
            Wife,               //Жена
            Child,              //Дети
            Max
        }

        public enum ItemFilterTypes
        {
            Default,
            Materials,
            Weapons,
            Ammo,
            Food,
            Population
        }

        /// <summary>
        /// Представляет описание предмета (тип предмета и список предметов, необходимых для его создания)
        /// </summary>
        public class Item
        {  
            private ItemTypes m_Type;                           //Тип предмета
            private int m_TicksToCreate;                        //Количество шагов для создания
            private List<ItemFilterTypes> m_FilterTypes;        //Фильтр для определения типа предмета (для вывода во вкладках и использования)
            private List<ItemAmountContainer> m_RequiredItems;  //Количество предметов, необходимые для создания этого предмета

            public ItemTypes Type => m_Type;
            public int TicksToCreate => m_TicksToCreate;
            public ItemAmountContainer[] RequiredItems => m_RequiredItems.ToArray();

            public Item(ItemTypes type, int ticksToCreate, List<ItemFilterTypes> filterType)
            {
                m_Type = type;
                m_TicksToCreate = ticksToCreate;
                m_FilterTypes = new List<ItemFilterTypes>();

                for (int i = 0; i < filterType.Count; i++)
                    m_FilterTypes.Add(filterType[i]);

                m_RequiredItems = new List<ItemAmountContainer>();
            }

            /// <summary>
            /// Добавить предметы, необходимые для создания предмета
            /// </summary>
            /// <param name="itemType">Тип необходимого предмета</param>
            /// <param name="amount">Количество необходимого предмета</param>
            public void AddRequiredItem(ItemTypes itemType, int amount)
            {
                ItemAmountContainer requiredItem = new ItemAmountContainer(itemType, amount);
                m_RequiredItems.Add(requiredItem);
            }

            /// <summary>
            /// Принадлежит ли этот предмет указаному фильтру
            /// </summary>
            /// <param name="filterType">Тип фильтра</param>
            /// <returns>true если принадлежит</returns>
            public bool MatchFilter(ItemFilterTypes filterType) => m_FilterTypes.Contains(filterType);

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(m_RequiredItems.Count * 10 + 20);

                //Вывести общие данные о предмете
                strBuilder.AppendFormat("Item: {0}. Ticks: {1}", m_Type, m_TicksToCreate);

                //Вывести необходимые для создания предмета предметы
                if (m_RequiredItems.Count > 0)
                {
                    strBuilder.AppendFormat("\nRequired Items {0}:\n", m_RequiredItems.Count);
                    for (int i = 0; i < m_RequiredItems.Count; i++)
                        strBuilder.AppendFormat(" - {0}\n", m_RequiredItems[i].ToString());
                }

                return strBuilder.ToString();
            }
        }

        /// <summary>
        /// Структура для сохранения количества предметов, определенного типа
        /// </summary>
        public class ItemAmountContainer
        {
            private ItemTypes m_Type;
            private int m_Amount;

            public ItemTypes Type => m_Type;
            public int Amount => m_Amount;

            /// <summary>
            /// Определенное количество предмета
            /// </summary>
            public ItemAmountContainer(ItemTypes type, int amount)
            {
                m_Type = type;
                m_Amount = amount;
            }

            /// <summary>
            /// Единица предмета
            /// </summary>
            public ItemAmountContainer(ItemTypes type)
            {
                m_Type = type;
                m_Amount = 1;
            }

            public void AddAmount(int amount)
            {
                m_Amount += amount;
            }

            public void RemoveAmount(int amount)
            {
                m_Amount -= amount;

                if (m_Amount < 0)
                    m_Amount = 0;
            }

            public override string ToString()
            {
                return string.Format("Type: {0}. Amount: {1}", m_Type, m_Amount);
            }
        }
        #endregion
    }
}

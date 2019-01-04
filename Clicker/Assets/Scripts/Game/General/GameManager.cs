using clicker.datatables;
using clicker.general.ui;
using UnityEngine;

namespace clicker.general
{
    [RequireComponent(typeof(UIManager))]
    [RequireComponent(typeof(LocalItemsDataEditor))]
    [RequireComponent(typeof(items.ItemsFactory))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public items.ItemsFactory CraftItemFactory { get; private set; }
        public UIManager UIManager { get; private set; }

        public account.Account PlayerAccount;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            DataTableItems.SetData(GetComponent<LocalItemsDataEditor>().Data_Items);
            DataTableWeapons.SetData(GetComponent<LocalWeaponsDataEditor>().Data_Weapons);

            UIManager = GetComponent<UIManager>();

            CraftItemFactory = GetComponent<items.ItemsFactory>();
            CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

            //Создать акканту
            PlayerAccount = new account.Account();
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 2);
        }


        void ItemCrafted_Handler(DataTableItems.ItemTypes type)
        {
            //Отнять предметы, необходимые для создания
            DataTableItems.Item itemData = DataTableItems.GetIemDataByType(type);
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
                PlayerAccount.Inventory.RemoveItem(itemData.RequiredItems[i].Type, itemData.RequiredItems[i].Amount);

            //Добавить созданный предмет
            PlayerAccount.Inventory.AddItem(type);
        }
    }
}

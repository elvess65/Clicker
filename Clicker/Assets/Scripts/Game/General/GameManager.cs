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

            UIManager = GetComponent<UIManager>();

            CraftItemFactory = GetComponent<items.ItemsFactory>();
            CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

            //Создать акканту
            PlayerAccount = new account.Account();
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 2);
        }

        void ItemCrafted_Handler(DataTableItems.ItemTypes type)
        {
            PlayerAccount.Inventory.AddItem(type);
        }
    }
}

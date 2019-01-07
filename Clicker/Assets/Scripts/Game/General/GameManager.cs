using clicker.account;
using clicker.battle;
using clicker.datatables;
using clicker.general.ui;
using clicker.items;
using UnityEngine;

namespace clicker.general
{
    [RequireComponent(typeof(ItemsFactory))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public UIManager Manager_UI;
        public BattleManager Manager_Battle;
        public InputManager Manager_Input;

        public ItemsFactory CraftItemFactory { get; private set; }   
        public Account PlayerAccount { get; private set; }

        void Awake()
        {
            //TODO: Переместить в раздел иницилазизации данных
            DataTableItems.SetData(GetComponent<LocalItemsDataEditor>().Data_Items);
            DataTableWeapons.SetData(GetComponent<LocalWeaponsDataEditor>().Data_Weapons);

            Instance = this;
        }

        void Start()
        {
            //TODO: Переместить в раздел иницилазизации данных
            //Создать акканту
            PlayerAccount = new Account();
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 2);

            //Создание предметов
            CraftItemFactory = GetComponent<ItemsFactory>();
            CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

            //UI
            Manager_UI.Init();

            //Battle
            DataTableItems.ItemTypes[] selectedWeapons = new DataTableItems.ItemTypes[2];
            selectedWeapons[0] = Account.AccountInventory.DEFAULT_ITEM;
            selectedWeapons[1] = DataTableItems.ItemTypes.Stone;

            Manager_Battle.Init(selectedWeapons);
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

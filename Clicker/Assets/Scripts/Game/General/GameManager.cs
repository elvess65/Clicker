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
      

        void Awake()
        {
            Instance = this;

            //Создание предметов
            CraftItemFactory = GetComponent<ItemsFactory>();
            CraftItemFactory.OnItemCrafted += ItemCrafted_Handler;

            //UI
            Manager_UI.Init();
        }

        void Start()
        {
            //Battle
            //TODO: Move to other place
            DataTableItems.ItemTypes[] selectedWeapons = new DataTableItems.ItemTypes[2];
            selectedWeapons[0] = Account.AccountInventory.DEFAULT_ITEM;
            selectedWeapons[1] = DataTableItems.ItemTypes.Stone;
            int playerHP = 20;

            Manager_Battle.Init(selectedWeapons, playerHP);
        }

        void ItemCrafted_Handler(DataTableItems.ItemTypes type)
        {
            //Отнять предметы, необходимые для создания
            DataTableItems.Item itemData = DataTableItems.GetIemDataByType(type);
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
                DataManager.Instance.PlayerAccount.Inventory.RemoveItem(itemData.RequiredItems[i].Type, itemData.RequiredItems[i].Amount);

            //Добавить созданный предмет
            DataManager.Instance.PlayerAccount.Inventory.AddItem(type);
        }
    }
}

using clicker.account;
using clicker.datatables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace clicker.general
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        public Account PlayerAccount { get; private set; }

        private iItemsDataLoader m_ItemsDataLoader;
        private iWeaponsDataLoader m_WeaponsDataLoader;
        private iLevelDataLoader m_LevelsDataLoader;
        private iPeriodicDataLoader m_PeriodicDataLoader;

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;

            DontDestroyOnLoad(this); 
        }

        void Start()
        {
            Initialize();
            StartCoroutine(Wait());
        }

        //Иммитация сохранения
        public static DataTableItems.ItemTypes[] SELECTED_WPN;  
        public static DataTableItems.ItemTypes[] SELECTED_FOOD; 
        public Dictionary<DataTableItems.ItemFilterTypes, int> BAGS;
        public static int PLAYER_HP = 20;                      
        public static int CRAFT_TIME = 15;                      
        public static int MAX_FOOD_IN_SLOT = 5;                

        void Initialize()
        {
            //Иммитация получения сохранения
            int accountID = 1;
            DataTableLevels.AgeTypes age = DataTableLevels.AgeTypes.FirstAge;
            int level = 0;

            int defaultSlotsCount = 1;
            SELECTED_WPN = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_WPN.Length; i++)
                SELECTED_WPN[i] = Account.AccountInventory.DEFAULT_ITEM;

            defaultSlotsCount = 2;
            SELECTED_FOOD = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_FOOD.Length; i++)
                SELECTED_FOOD[i] = DataTableItems.ItemTypes.Max;

            defaultSlotsCount = 3;
            BAGS = new Dictionary<DataTableItems.ItemFilterTypes, int>();
            BAGS.Add(DataTableItems.ItemFilterTypes.Food, defaultSlotsCount);

            //Инициализировать загрузчики данных
            m_ItemsDataLoader = new ItemsDataLoader_Local();
            m_WeaponsDataLoader = new WeaponsDataLoader_Local();
            m_LevelsDataLoader = new LevelDataLoader_Local();
            m_PeriodicDataLoader = new PeriodicDataLoader_Local();

            //Загрузить данные
            DataTableItems.SetData(m_ItemsDataLoader.GetData(accountID), m_ItemsDataLoader.GetIgnoreData(accountID));
            DataTableWeapons.SetData(m_WeaponsDataLoader.GetData(accountID));
            DataTableLevels.SetData(m_LevelsDataLoader.GetData(accountID));
            DataTablePeriodic.SetData(m_PeriodicDataLoader.GetData(accountID));

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, SELECTED_WPN, SELECTED_FOOD, BAGS);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 1);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Berries, 3);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.GrilledMeat, 3);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Water, 50);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Meat, 50);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Fire, 50);

            Debug.Log("Initialize");
        }

        public void ResetProgress()
        {
            PlayerAccount.ResetProgress();

            //Иммитация получения сохранения
            int accountID = 1;
            DataTableLevels.AgeTypes age = DataTableLevels.AgeTypes.FirstAge;
            int level = 0;

            int defaultSlotsCount = 1;
            SELECTED_WPN = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_WPN.Length; i++)
                SELECTED_WPN[i] = Account.AccountInventory.DEFAULT_ITEM;

            defaultSlotsCount = 1;
            SELECTED_FOOD = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_FOOD.Length; i++)
                SELECTED_FOOD[i] = DataTableItems.ItemTypes.Max;

            defaultSlotsCount = 3;
            BAGS = new Dictionary<DataTableItems.ItemFilterTypes, int>();
            BAGS.Add(DataTableItems.ItemFilterTypes.Food, defaultSlotsCount);

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, SELECTED_WPN, SELECTED_FOOD, BAGS);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 1);
        }


        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(1);
        }
    }
}

/*
 U - use weapon
 S - add slot
 */
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
        private iFoodDataLoader m_FoodDataLoader;
        private iUpgradesDataLoader m_UpgradesDataLoader;

        //Иммитация сохранения
        public static DataTableItems.ItemTypes[] SELECTED_WPN;
        public static DataTableItems.ItemTypes[] SELECTED_FOOD;
        public Dictionary<DataTableItems.ItemFilterTypes, int> BAGS;
        Dictionary<DataTableUpgrades.UpgradeTypes, (int, int)> UPGRADES;
        public static int PLAYER_HP = 20;
        public static int CRAFT_TIME = 15;
        public static int MAX_FOOD_IN_SLOT = 5;

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

        void Initialize()
        {
            //Иммитация получения сохранения
            int accountID = 1;
            DataTableLevels.AgeTypes age = DataTableLevels.AgeTypes.FirstAge;
            int level = 0;
            int coins = 0;

            //Слоты оружия
            int defaultSlotsCount = 1;
            SELECTED_WPN = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_WPN.Length; i++)
                SELECTED_WPN[i] = Account.AccountInventory.DEFAULT_ITEM;

            //Слоты еды
            defaultSlotsCount = 2;
            SELECTED_FOOD = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_FOOD.Length; i++)
                SELECTED_FOOD[i] = DataTableItems.ItemTypes.Max;

            //Сумки
            BAGS = new Dictionary<DataTableItems.ItemFilterTypes, int>();
            //Сумки с едой
            defaultSlotsCount = 3;
            BAGS.Add(DataTableItems.ItemFilterTypes.Food, defaultSlotsCount);
            //Сумки с оружием
            defaultSlotsCount = 2;
            BAGS.Add(DataTableItems.ItemFilterTypes.Weapons, defaultSlotsCount);

            //Улучшения
            UPGRADES = new Dictionary<DataTableUpgrades.UpgradeTypes, (int, int)>();
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.WeaponSlot,    (1, 0));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.WeaponBag,     (1, 0));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.FoodSlot,      (1, 0));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.FoodBag,       (1, 0));

            //Инициализировать загрузчики данных
            m_ItemsDataLoader = new ItemsDataLoader_Local();
            m_WeaponsDataLoader = new WeaponsDataLoader_Local();
            m_LevelsDataLoader = new LevelDataLoader_Local();
            m_PeriodicDataLoader = new PeriodicDataLoader_Local();
            m_FoodDataLoader = new FoodDataLoader_Local();
            m_UpgradesDataLoader = new UpgradesDataLoader_Local();

            //Загрузить данные
            DataTableItems.SetData(m_ItemsDataLoader.GetData(accountID), m_ItemsDataLoader.GetIgnoreData(accountID));
            DataTableWeapons.SetData(m_WeaponsDataLoader.GetData(accountID));
            DataTableLevels.SetData(m_LevelsDataLoader.GetData(accountID));
            DataTablePeriodic.SetData(m_PeriodicDataLoader.GetData(accountID));
            DataTableFood.SetData(m_FoodDataLoader.GetData(accountID));
            DataTableUpgrades.SetData(m_UpgradesDataLoader.GetData(accountID));

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, coins, SELECTED_WPN, SELECTED_FOOD, BAGS, UPGRADES);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 1);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Berries, 2);
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
            int coins = 0;

            //Слоты оружия
            int defaultSlotsCount = 1;
            SELECTED_WPN = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_WPN.Length; i++)
                SELECTED_WPN[i] = Account.AccountInventory.DEFAULT_ITEM;

            //Слоты еды
            defaultSlotsCount = 1;
            SELECTED_FOOD = new DataTableItems.ItemTypes[defaultSlotsCount];
            for (int i = 0; i < SELECTED_FOOD.Length; i++)
                SELECTED_FOOD[i] = DataTableItems.ItemTypes.Max;

            //Сумки
            BAGS = new Dictionary<DataTableItems.ItemFilterTypes, int>();
            //Сумки с едой
            defaultSlotsCount = 3;
            BAGS.Add(DataTableItems.ItemFilterTypes.Food, defaultSlotsCount);
            //Сумки с оружием
            defaultSlotsCount = 2;
            BAGS.Add(DataTableItems.ItemFilterTypes.Weapons, defaultSlotsCount);

            //Улучшения
            UPGRADES = new Dictionary<DataTableUpgrades.UpgradeTypes, (int, int)>();
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.WeaponSlot, (1, 3));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.WeaponBag, (1, 0));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.FoodSlot, (1, 0));
            UPGRADES.Add(DataTableUpgrades.UpgradeTypes.FoodBag, (1, 0));

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, coins, SELECTED_WPN, SELECTED_FOOD, BAGS, UPGRADES);
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
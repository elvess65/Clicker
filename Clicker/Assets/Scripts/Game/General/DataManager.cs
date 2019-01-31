using clicker.account;
using clicker.datatables;
using System.Collections;
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

        public static DataTableItems.ItemTypes[] SELECTED_WPN;  //Иммитация сохранения
        public static int PLAYER_HP = 20;                       //Иммитация сохранения
        public static int CRAFT_TIME = 15;                      //Иммитация сохранения

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

            //Инициализировать загрузчики данных
            m_ItemsDataLoader = new ItemsDataLoader_Local();
            m_WeaponsDataLoader = new WeaponsDataLoader_Local();
            m_LevelsDataLoader = new LevelDataLoader_Local();

            //Загрузить данные
            DataTableItems.SetData(m_ItemsDataLoader.GetData(accountID));
            DataTableWeapons.SetData(m_WeaponsDataLoader.GetData(accountID));
            DataTableLevels.SetData(m_LevelsDataLoader.GetData(accountID));

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, SELECTED_WPN);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 1);

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

            //Создать акканту
            PlayerAccount = new Account(accountID, PLAYER_HP, CRAFT_TIME, age, level, SELECTED_WPN);
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
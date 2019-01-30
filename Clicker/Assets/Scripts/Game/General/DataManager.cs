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
        }

        void Initialize()
        {
            //Должно быть перемещено в поолучение данных
            int accountID = 1;
            int weaponSlots = 1;
            DataTableLevels.AgeTypes age = DataTableLevels.AgeTypes.FirstAge;
            int level = 0;

            //Инициализировать загрузчики данных
            m_ItemsDataLoader = new ItemsDataLoader_Local();
            m_WeaponsDataLoader = new WeaponsDataLoader_Local();
            m_LevelsDataLoader = new LevelDataLoader_Local();

            //Загрузить данные
            DataTableItems.SetData(m_ItemsDataLoader.GetData(accountID));
            DataTableWeapons.SetData(m_WeaponsDataLoader.GetData(accountID));
            DataTableLevels.SetData(m_LevelsDataLoader.GetData(accountID));

            //Создать акканту
            PlayerAccount = new Account(accountID, age, level, weaponSlots);
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 1);

            Debug.Log("Initialize");

            StartCoroutine(Wait());
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
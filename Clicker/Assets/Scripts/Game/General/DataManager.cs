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
            //Инициализировать загрузчики данных
            m_ItemsDataLoader = new ItemsDataLoader_Local();
            m_WeaponsDataLoader = new WeaponsDataLoader_Local();

            //Загрузить данные
            DataTableItems.SetData(m_ItemsDataLoader.GetData(0));
            DataTableWeapons.SetData(m_WeaponsDataLoader.GetData(0));

            //Создать акканту
            PlayerAccount = new Account();
            PlayerAccount.Inventory.AddItem(DataTableItems.ItemTypes.Stone, 2);

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
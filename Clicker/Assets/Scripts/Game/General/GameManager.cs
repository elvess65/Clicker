using clicker.account;
using clicker.battle;
using clicker.datatables;
using clicker.general.ui;
using clicker.general.ui.windows;
using clicker.items;
using FrameworkPackage.UI.Windows;
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
        public AssetsLibrary AssetsLibrary;

        public ItemsFactory CraftItemFactory { get; private set; }   
        public bool GameIsActive { get; private set; }

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
            Manager_Battle.Init(DataManager.Instance.PlayerAccount.HP, DataManager.Instance.PlayerAccount.Age, DataManager.Instance.PlayerAccount.Level);

            GameIsActive = true;
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


        public void HandleGameOver()
        {
            GameIsActive = false;

            //Очистить прогресс игрока
            DataManager.Instance.ResetProgress();

            //Спрятать все окна
            Manager_UI.WindowsManager.HideAllWindows();

            //Показать окно
            UIWindow_CloseButton wnd = Manager_UI.WindowsManager.ShowWindow(Manager_UI.WindowsManager.UIWindow_GameOver) as UIWindow_CloseButton;
            wnd.Button_Close.onClick.AddListener(() =>
            {
                ReloadLevel();
            });
        }

        public void HandleFinishLevel()
        {
            //Поставить игру на паузу
            GameIsActive = false;

            //Повысить уровень
            DataManager.Instance.PlayerAccount.IncrementLevel();

            //Сохранить текущее состояние выбранного оружия
            //Иммитация сохранения
            DataManager.SELECTED_WPN = DataManager.Instance.PlayerAccount.Inventory.SelectedWeapon.ToArray();

            //Спрятать все окна
            Manager_UI.WindowsManager.HideAllWindows();

            //Показать окно повышения уровня
            UIWindow_CloseButton wnd = Manager_UI.WindowsManager.ShowWindow(Manager_UI.WindowsManager.UIWindow_LevelFinished) as UIWindow_CloseButton;
            wnd.Button_Close.onClick.AddListener(() => 
            {
                //Запустить окно, которое показывает скольво времени для крафта осталось
                UIWindow_CraftTime craftTimeWnd = Manager_UI.WindowsManager.ShowWindowWithoutFade(Manager_UI.WindowsManager.UIWindow_CraftTime) as UIWindow_CraftTime;
                craftTimeWnd.OnUIHided += ReloadLevel;
                craftTimeWnd.Init(DataManager.Instance.PlayerAccount.CraftTime);
            });
        }

        void ReloadLevel()
        {
            Manager_Battle.SelectedWeaponManager.UnscribeFromGlobalEvents();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}

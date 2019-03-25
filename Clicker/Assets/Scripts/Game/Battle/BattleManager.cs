using clicker.battle.character;
using clicker.datatables;
using clicker.general;
using UnityEngine;

namespace clicker.battle
{
    /// <summary>
    /// Отвечает за бой
    /// </summary>
    [RequireComponent(typeof(WeaponManager))]
    [RequireComponent(typeof(FoodManager))]
    public class BattleManager : MonoBehaviour
    {
        public WeaponManager SelectedWeaponManager { get; private set; }
        public FoodManager SelectedFoodManager { get; private set; }
        public PopulationManager PopulationManager { get; private set; }
        public Player Player;

        private level.LevelController m_LevelController;

        public void Init(int playerHP, DataTableLevels.AgeTypes age, int level)
        {
            //Выбранное оружие
            SelectedWeaponManager = GetComponent<WeaponManager>();
            SelectedWeaponManager.Init();

            //Выбранная еда
            SelectedFoodManager = GetComponent<FoodManager>();
            SelectedFoodManager.Init();

            //Инициализация игрока
            Player.OnCharacterDestroyed += PlayerDestroyedHandler;
            Player.Init(playerHP);
            Player.HPController.TakeDamage(10);

            //Инициализация населения
            PopulationManager = GetComponent<PopulationManager>();
            PopulationManager.Init();

            //UI собранной награды
            GameManager.Instance.Manager_UI.CreateUI_Coins(GameManager.Instance.Manager_UI.UIParent_LeftTop, DataManager.Instance.PlayerAccount.Coins);

            //Инициализация ввода для атаки
            GameManager.Instance.Manager_Input.OnInput += InputHandler;
            GameManager.Instance.Manager_Input.EnableInput(true);

            //Начало создания врагов
            m_LevelController = GetComponent<level.LevelController>();
            m_LevelController.OnLevelFinished += LevelFinishedHandler;
            m_LevelController.Init(age, level);
            m_LevelController.StartSpawn();
        }


        void LevelFinishedHandler()
        {
            m_LevelController.OnLevelFinished -= LevelFinishedHandler;

            GameManager.Instance.HandleFinishLevel();
        }

        void PlayerDestroyedHandler(Character character)
        {
            character.OnCharacterDestroyed -= PlayerDestroyedHandler;

            GameManager.Instance.HandleGameOver();
        }

        void InputHandler(Vector3 mousePos)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, 1000))
            {
                //Нанести урон врагу
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.HPController.TakeDamage(SelectedWeaponManager.UseItem());
                    return;
                }

                //Поднять объект
                PickableObject pickableObj = hit.collider.gameObject.GetComponent<PickableObject>();
                if (pickableObj != null)
                {
                    pickableObj.Pick();
                    return;
                }
            }
        }
    }
}

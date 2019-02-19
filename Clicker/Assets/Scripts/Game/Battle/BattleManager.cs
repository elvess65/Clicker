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
    public class BattleManager : MonoBehaviour
    {
        public WeaponManager SelectedWeaponManager { get; private set; }
        public Player Player;

        private level.LevelController m_LevelController;

        public void Init(int playerHP, DataTableLevels.AgeTypes age, int level)
        {
            //Выбранное оружие
            SelectedWeaponManager = GetComponent<WeaponManager>();
            SelectedWeaponManager.Init();

            //Выбранная еда
            GameManager.Instance.Manager_UI.CreateFoodSlots(new DataTableItems.ItemTypes[] { DataTableItems.ItemTypes.Max },
                                                             GameManager.Instance.Manager_UI.UIParent_MiddleRight,
                                                             true,
                                                             true);

            //Инициализация игрока
            Player.OnCharacterDestroyed += PlayerDestroyedHandler;
            Player.Init(playerHP);

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
                    enemy.HPController.TakeDamage(SelectedWeaponManager.UseWeapon());
            }
        }
    }
}

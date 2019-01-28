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
        public LayerMask EnemyLayerMask;
        public Player Player;

        private level.LevelController m_LevelController;

        public void Init(DataTableItems.ItemTypes[] selectedWeapons, int playerHP)
        {
            //Выбранное оружие
            SelectedWeaponManager = GetComponent<WeaponManager>();
            SelectedWeaponManager.Init(selectedWeapons);

            //Инициализация игрока
            Player.Init(playerHP);

            //Инициализация ввода для атаки
            GameManager.Instance.Manager_Input.OnInput += InputHandler;
            GameManager.Instance.Manager_Input.EnableInput(true);

            //Начало создания врагов
            m_LevelController = GetComponent<level.LevelController>();
            m_LevelController.Init(0, 0);
            m_LevelController.StartSpawn();
        }

        void InputHandler(Vector3 mousePos)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit, 1000, EnemyLayerMask))
            {
                //Нанести урон врагу
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.HPController.TakeDamage(SelectedWeaponManager.UseWeapon());
            }
        }
    }
}

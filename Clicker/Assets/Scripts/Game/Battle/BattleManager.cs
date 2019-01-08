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

        public void Init(DataTableItems.ItemTypes[] selectedWeapons)
        {
            SelectedWeaponManager = GetComponent<WeaponManager>();
            SelectedWeaponManager.Init(selectedWeapons);

            GameManager.Instance.Manager_Input.OnInput += InputHandler;
            GameManager.Instance.Manager_Input.EnableInput(true);
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

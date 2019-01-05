using clicker.datatables;
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

        public void Init(DataTableItems.ItemTypes[] selectedWeapons)
        {
            SelectedWeaponManager = GetComponent<WeaponManager>();
            SelectedWeaponManager.Init(selectedWeapons);
        }
    }
}

using clicker.battle.HP;
using clicker.general;
using UnityEngine;

namespace clicker.battle.character
{
    /// <summary>
    /// Класс игрока
    /// </summary>
    public class Player : Character
    {
        protected override void DestroyObjectHandler()
        {
            Debug.Log("Game over");
        }

        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.PlayerHPBarControllerPrefab;
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                HPController.TakeDamage(enemy.Damage);
                Destroy(enemy.gameObject);
            }
        }
    }
}

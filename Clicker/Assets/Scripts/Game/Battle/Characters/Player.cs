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
        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.PlayerHPBarControllerPrefab;
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Нанести урон игроку
                HPController.TakeDamage(enemy.Damage);

                //Если игрок не был уничтожен
                if (HPController.CurrentProgress > 0)
                    enemy.DestroyEnemyOnCollisionWithPlayer();
            }
        }
    }
}

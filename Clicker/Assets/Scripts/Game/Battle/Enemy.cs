using clicker.battle.HP;
using clicker.general;
using FrameworkPackage.iTween;
using UnityEngine;

namespace clicker.battle.character
{
    /// <summary>
    /// Класс врага
    /// </summary>
    public class Enemy : Character
    {
        public iTweenPathMoveController PathMoveController;

        public int Damage => 10;

        public void Init(int health, int speed, iTweenPath path)
        {
            Init(health);

            PathMoveController.StartMove(speed, path);
        }

        public void DestroyEnemyOnCollisionWithPlayer()
        {
            DestroyObjectHandler();
            Destroy(gameObject);
        }


        protected override void DestroyObjectHandler()
        {
            base.DestroyObjectHandler();

            Destroy(gameObject);
        }

        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab;
        }
    }
}

using clicker.battle.HP;
using clicker.general;
using UnityEngine;

namespace clicker.battle.character
{
    /// <summary>
    /// Класс врага
    /// </summary>
    public class Enemy : Character
    {
        public int Damage => 10;

        protected override void DestroyObjectHandler()
        {
            base.DestroyObjectHandler();

            Destroy(gameObject);
        }

        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab;
        }

        private void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * 2);
        }
    }
}

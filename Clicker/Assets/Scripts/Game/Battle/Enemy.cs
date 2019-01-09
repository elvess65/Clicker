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
        protected override void DestroyObjectHandler()
        {
            Destroy(gameObject);
        }

        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab;
        }

        public int Damage => 10;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Init(10);
        }
    }
}

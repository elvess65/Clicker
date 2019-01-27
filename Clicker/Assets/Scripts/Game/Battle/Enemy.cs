using clicker.battle.HP;
using clicker.general;
using System.Collections;

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
            Destroy(gameObject);
        }

        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab;
        }
        
        //TODO: Move to spawn
        void Start()
        {
            Init(10);
        }
    }
}

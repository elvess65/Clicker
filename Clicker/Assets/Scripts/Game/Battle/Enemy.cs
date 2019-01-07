using clicker.battle.HP;
using UnityEngine;

namespace clicker.battle
{
    /// <summary>
    /// Класс врага
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        public HPController HPController { get; private set; }

        public void Init(int health)
        {
            HPController = new HPController(health);
        }

        void Update()
        {

        }
    }
}

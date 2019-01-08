using clicker.battle.HP;
using UnityEngine;

namespace clicker.battle
{
    /// <summary>
    /// Класс врага
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        public Transform HPBarParent;

        public HPController HPController { get; private set; }

        public void Init(int health)
        {
            HPController = new HPController(health, HPBarParent);
            HPController.OnDestroyObject += DestroyObjectHandler;
        }

        void DestroyObjectHandler()
        {
            Destroy(gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Init(10);
        }
    }
}

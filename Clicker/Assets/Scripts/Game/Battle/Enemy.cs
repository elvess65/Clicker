using clicker.battle.HP;
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Init(10);
        }
    }
}

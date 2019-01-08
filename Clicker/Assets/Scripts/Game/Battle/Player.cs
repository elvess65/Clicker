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
    }
}

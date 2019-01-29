using clicker.battle.HP;
using UnityEngine;

namespace clicker.battle.character
{
    /// <summary>
    /// Базовый класс для всех персонажей (враг и игрок)
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        public System.Action<Character> OnCharacterDestroyed;

        public Transform HPBarParent;

        public HPController HPController { get; private set; }


        public void Init(int health)
        {
            HPController = new HPController(health, GetHPBarControllerPrefab(), HPBarParent);
            HPController.OnDestroyObject += DestroyObjectHandler;
        }


        protected abstract HPBarController GetHPBarControllerPrefab();

        protected virtual void DestroyObjectHandler()
        {
            OnCharacterDestroyed?.Invoke(this);
        }
    }
}

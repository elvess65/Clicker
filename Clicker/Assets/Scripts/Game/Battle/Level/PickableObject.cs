using UnityEngine;

namespace clicker.battle
{
    /// <summary>
    /// Базовый класс для всех объектов, которые можно поднять
    /// </summary>
    public abstract class PickableObject : MonoBehaviour
    {
        public abstract void Pick();
    }
}

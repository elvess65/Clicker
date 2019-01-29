using clicker.general;
using UnityEngine;
using UnityEngine.EventSystems;

namespace clicker.battle
{
    public class InputManager : MonoBehaviour
    {
        public System.Action<Vector3> OnInput;

        private bool m_IsActive = false;

        void Update()
        {
            if (GameManager.Instance.GameIsActive && m_IsActive && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                OnInput?.Invoke(Input.mousePosition);
        }

        public void EnableInput(bool inputEnabled)
        {
            m_IsActive = inputEnabled;
        }
    }
}

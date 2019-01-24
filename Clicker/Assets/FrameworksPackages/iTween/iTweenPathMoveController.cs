using UnityEngine;

namespace FrameworkPackage.iTween
{
    [RequireComponent(typeof(iTweenPath))]
    public class iTweenPathMoveController : MonoBehaviour
    {
        public System.Action OnArrived;
        public iTween.EaseType EaseType = iTween.EaseType.linear;

        private bool m_IsMoving = false;
        private iTweenPath m_PathController;

        void Start()
        {
            m_PathController = GetComponent<iTweenPath>();
        }

        /// <summary>
        /// Начало движения объекта
        /// </summary>
        /// <param name="speed">Скорость объекта</param>
        public void StartMove(float speed)
        {
            if (m_IsMoving)
                return;

            m_IsMoving = true;
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(m_PathController.pathName),
                                              "speed", speed,
                                              "easetype", EaseType,
                                              "oncompletetarget", gameObject,
                                              "oncomplete", "ArrivedHandler"));
        }

        /// <summary>
        /// Остановить движение
        /// </summary>
        public void StopMoving()
        {
            GetComponent<iTween>().enabled = false;
        }

        /// <summary> 
        /// Обработчик окончания движения
        /// </summary>
        void ArrivedHandler()
        {
            m_IsMoving = false;

            OnArrived?.Invoke();
        }
    }
}

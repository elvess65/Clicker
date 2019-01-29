using UnityEngine;

namespace FrameworkPackage.iTween
{
    public class iTweenPathMoveController : MonoBehaviour
    {
        public System.Action OnArrived;

        public iTween.EaseType EaseType = iTween.EaseType.linear;

        private bool m_IsMoving = false;     

        /// <summary>
        /// Начало движения объекта
        /// </summary>
        /// <param name="speed">Скорость объекта</param>
        public void StartMove(float speed, iTweenPath path)
        {
            if (m_IsMoving)
                return;

            m_IsMoving = true;
            Debug.Log(gameObject);
            Debug.Log(path.pathName);
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(path.pathName),
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

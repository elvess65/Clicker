using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkPackage.PathCreation
{
    /// <summary>
    /// Перемещение по указанному пути
    /// </summary>
    public class MovePathController : MonoBehaviour
    {
        private PathCreator m_PathCreator;
        private bool m_IsStarted = false;
        private float m_DistanceTravelled;
        private float m_Speed;
        private float m_SpeedMltp;         

        /// <summary>
        /// Начать передвигаться по пути
        /// </summary>
        public void StartMoveAlongPath(PathCreator pathCreator, float speed, float speedMltp)
        {
            m_PathCreator = pathCreator;
            m_Speed = speed;
            m_DistanceTravelled = 0;

            SetMultiplyer(speedMltp);

            m_IsStarted = true;
        }

        /// <summary>
        /// Изменить множитель времени
        /// </summary>
        public void SetMultiplyer(float speedMltp)
        {
            m_SpeedMltp = speedMltp;
        }

        /// <summary>
        /// Остановить движение по пути
        /// </summary>
        public void StopMoveAlongPath()
        {
            m_IsStarted = false;
        }


        void Update()
        {
            if (m_IsStarted)
            {
                m_DistanceTravelled += Time.deltaTime * m_Speed * m_SpeedMltp;

                transform.position = m_PathCreator.path.GetPointAtDistance(m_DistanceTravelled);
                transform.rotation = m_PathCreator.path.GetRotationAtDistance(m_DistanceTravelled);

                //Quaternion rot = m_PathCreator.path.GetRotation(t / tt);
                //Debug.Log(rot.eulerAngles);
                //transform.localEulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y - 90, rot.eulerAngles.z);
                //transform.rotation = rot * Quaternion.Euler(0, -90, 0);
            }
        }
    }
}

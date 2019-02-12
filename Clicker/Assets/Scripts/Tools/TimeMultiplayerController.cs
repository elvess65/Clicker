using FrameworkPackage.Utils;
using UnityEngine;

namespace clicker.tools
{
    public class TimeMultiplayerController : MonoBehaviour
    {
        public System.Action<float> OnMultiplayerValueChanged;

        private InterpolationData<float> m_LerpData;

        private const float m_MULTIPLAYER_TRANSITION_TIME = 0.5f;
        private const float m_MULTIPLAYER_INIT_VALUE = 1;

        public float CurValue { get; private set; }

        public void Init()
        {
            m_LerpData = new InterpolationData<float>();
            m_LerpData.TotalTime = m_MULTIPLAYER_TRANSITION_TIME;
            CurValue = m_MULTIPLAYER_INIT_VALUE;
        }

        /// <summary>
        /// Начать плавно изменять значение до указанного
        /// </summary>
        public void StartChangeMultiplayer(float targetValue)
        {
            m_LerpData.From = CurValue;
            m_LerpData.To = targetValue;

            m_LerpData.Start();
        }

        /// <summary>
        /// Начать плавно восстанавливать значение до начального
        /// </summary>
        public void StartResetingMultiplayer()
        {
            StartChangeMultiplayer(m_MULTIPLAYER_INIT_VALUE);
        }

        /// <summary>
        /// Мгновенно вернуть значение до начального
        /// </summary>
        public void ImmediateResetMultiplayer()
        {
            CurValue = m_MULTIPLAYER_INIT_VALUE;
            OnMultiplayerValueChanged?.Invoke(CurValue);
        }


        void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                CurValue = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
                OnMultiplayerValueChanged?.Invoke(CurValue);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();
                    CurValue = m_LerpData.To;

                    OnMultiplayerValueChanged?.Invoke(CurValue);
                }
            }

        }
    }
}
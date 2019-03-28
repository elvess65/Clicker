using clicker.general;
using FrameworkPackage.Utils;
using UnityEngine;

namespace clicker.battle
{
    public class PeriodicManager : MonoBehaviour
    {
        public System.Action<float> OnProgress;
        public System.Action OnPeriodFinished;

        private float m_ActionPeriod;
        private bool m_Loop = false;
        private bool m_WasStopped = false;
        private InterpolationData<float> m_LerpPeriod;

        public float Progress => m_LerpPeriod.Progress;
        public float Multiplayer { get; private set; }
        public bool WasStopped => m_WasStopped;

        public void Init(float actionPeriod, bool loop, float multiplayer = 1)
        {
            SetMultiplyer(multiplayer);

            m_Loop = loop;
            SetPeriod(actionPeriod);

            m_LerpPeriod = new InterpolationData<float>(actionPeriod);
            m_LerpPeriod.From = 0;
            m_LerpPeriod.To = 1;
        }

        public void SetMultiplyer(float multiplayer)
        {
            Multiplayer = multiplayer;
        }

        public void SetPeriod(float actionPeriod)
        {
            m_ActionPeriod = actionPeriod;
            m_LerpPeriod.TotalTime = m_ActionPeriod;
        }

        public void StartPeriod()
        {
            if (m_WasStopped)
                m_WasStopped = false;

            m_LerpPeriod.Start();
        }

        public void StopPeriod()
        {
            m_LerpPeriod.Stop();
            m_WasStopped = true;
        }


        void Update()
        {
            if (GameManager.Instance.GameIsActive && m_LerpPeriod.IsStarted)
            {
                m_LerpPeriod.Increment(Multiplayer);

                OnProgress?.Invoke(Progress);

                if (m_LerpPeriod.Overtime())
                {
                    OnPeriodFinished?.Invoke();

                    if (!m_WasStopped)
                    {
                        if (m_Loop)
                            StartPeriod();
                        else
                            m_LerpPeriod.Stop();
                    }
                }
            }
        }
    }
}

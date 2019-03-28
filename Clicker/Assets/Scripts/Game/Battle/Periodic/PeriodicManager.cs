using clicker.general;
using FrameworkPackage.Utils;
using UnityEngine;

namespace clicker.battle
{
    public class PeriodicManager : MonoBehaviour
    {
        public System.Action<float> OnProgress;
        public System.Action OnPeriodFinished;

        private bool m_Loop = false;
        private InterpolationData<float> m_LerpPeriod;

        public float Progress => m_LerpPeriod.Progress;
        public float Multiplayer { get; private set; }
        public bool WasStopped { get; private set; }

        public void Init(float actionPeriod, bool loop, float multiplayer = 1)
        {
            SetMultiplyer(multiplayer);

            m_LerpPeriod = new InterpolationData<float>();
            m_LerpPeriod.From = 0;
            m_LerpPeriod.To = 1;

            m_Loop = loop;

            SetPeriod(actionPeriod);
        }

        public void SetMultiplyer(float multiplayer)
        {
            Multiplayer = multiplayer;
        }

        public void SetPeriod(float actionPeriod)
        {
            m_LerpPeriod.TotalTime = actionPeriod;
        }

        public void StartPeriod()
        {
            if (WasStopped)
                WasStopped = false;

            m_LerpPeriod.Start();
        }

        public void StopPeriod()
        {
            m_LerpPeriod.Stop();
            WasStopped = true;
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

                    if (!WasStopped)
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

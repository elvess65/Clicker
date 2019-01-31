using FrameworkPackage.UI.Windows;
using FrameworkPackage.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_CraftTime : UIWindow_CloseButton
    {
        [Header("Images")]
        public Image Image_LeftTimeFG;

        private InterpolationData<float> m_LerpData;

        private const int m_TIME_TO_DOUBLE_DIGITS = 3;

        public void Init(int craftTime)
        {
            Init();

            m_LerpData = new InterpolationData<float>(craftTime);
            m_LerpData.From = 0;
            m_LerpData.To = 1;
            m_LerpData.Start();

            UpdateLeftTime(craftTime);
        }


        void UpdateLeftTime(float leftTime)
        {
            string pattern = "{0:0}s";
            if (leftTime <= m_TIME_TO_DOUBLE_DIGITS)
                pattern = "{0:0.0}s";

            Text_Main.text = string.Format(pattern, leftTime);
        }

        void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                UpdateLeftTime(m_LerpData.TotalTime - m_LerpData.CurTime);

                Image_LeftTimeFG.fillAmount = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    Hide();
                }
            }
        }
    }
}

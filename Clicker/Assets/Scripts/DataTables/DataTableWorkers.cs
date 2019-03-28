using System.Collections.Generic;
using System.Text;

namespace clicker.datatables
{
    public class DataTableWorkers
    {
        private static Dictionary<DataTableLevels.AgeTypes, Workers> m_Workers;

        public static void SetData(WorkersData[] data)
        {
            if (data == null)
                return;

            m_Workers = new Dictionary<DataTableLevels.AgeTypes, Workers>();

            for (int i = 0; i < data.Length; i++)
            {
                //Создать предмет
                Workers item = new Workers(data[i].AgeType, data[i].MaxWorkers, data[i].MaxWorkerLvl, data[i].BaseTickPeriod, data[i].LevelDelta);

                m_Workers.Add(data[i].AgeType, item);
            }
        }

        public static Workers GetWorkersData(DataTableLevels.AgeTypes ageType)
        {
            if (m_Workers.ContainsKey(ageType))
                return m_Workers[ageType];

            return null;
        }

        public static float GetTickPeriodForLvl(DataTableLevels.AgeTypes ageType, int lvl)
        {
            if (m_Workers.ContainsKey(ageType))
            {
                float baseTickPeriod = m_Workers[ageType].BaseTickPeriod;
                int maxWorkersLvl = m_Workers[ageType].MaxWorkerLvl;

                return baseTickPeriod - m_Workers[ageType].LevelDelta * (lvl - 1);
            }

            return 1;
        }


        #region Data Structures
        public class Workers
        {
            private DataTableLevels.AgeTypes m_AgeType;
            private int m_MaxWorkers;
            private int m_MaxWorkerLvl;
            private float m_BaseTickPeriod;
            private float m_LevelDelta;

            public DataTableLevels.AgeTypes AgeType => m_AgeType;
            public int MaxWorkers => m_MaxWorkers;
            public int MaxWorkerLvl => m_MaxWorkerLvl;
            public float BaseTickPeriod => m_BaseTickPeriod;
            public float LevelDelta => m_LevelDelta;

            public Workers(DataTableLevels.AgeTypes ageType, int maxWorkers, int maxWorkerLvl, float baseTickPeriod, float levelDelta)
            {
                m_AgeType = ageType;
                m_MaxWorkers = maxWorkers;
                m_MaxWorkerLvl = maxWorkerLvl;
                m_BaseTickPeriod = baseTickPeriod;
                m_LevelDelta = levelDelta;
            }

            public override string ToString()
            {
                StringBuilder strBuilder = new StringBuilder(50);

                //Вывести общие данные 
                strBuilder.AppendFormat("AgeType: {0}. MaxWorkers: {1}. MaxWorkerLvl: {2}. BaseTickPeriod {3}", AgeType, MaxWorkers, MaxWorkerLvl, BaseTickPeriod);

                return strBuilder.ToString();
            }
        }
        #endregion
    }
}

using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    interface iWorkersDataLoader
    {
        WorkersData[] GetData(int dataParam);
    }

    public class WorkersDataLoader_Local : iWorkersDataLoader
    {
        public WorkersData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalWorkersDataEditor>().Data_Workers;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct WorkersData
    {
        public DataTableLevels.AgeTypes AgeType;
        public int MaxWorkers;
        public int MaxWorkerLvl;
        public float BaseTickPeriod;
        public float LevelDelta;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);

            //Вывести общие данные 
            strBuilder.AppendFormat("AgeType: {0}. MaxWorkers: {1}. MaxWorkerLvl: {2}. BaseTickPeriod {3}", AgeType, MaxWorkers, MaxWorkerLvl, BaseTickPeriod);

            return strBuilder.ToString();
        }
    }
}

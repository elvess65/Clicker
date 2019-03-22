using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace clicker.datatables
{
    interface iUpgradesDataLoader
    {
        UpgradesData[] GetData(int dataParam);
    }

    public class UpgradesDataLoader_Local : iUpgradesDataLoader
    {
        public UpgradesData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalUpgradesDataEditor>().Data_Upgrades;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public struct UpgradesData
    {
        public DataTableUpgrades.UpgradeTypes Type;
        public float StepDelta;
        public int StepConst;
        public int[] LevelsData;

        public Dictionary<int, int> GetLevelsData()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            for (int i = 0; i < LevelsData.Length; i++)
                result.Add(i, LevelsData[i]);

            return result;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(50);

            //Вывести общие данные 
            strBuilder.AppendFormat("Weapon: {0}. DataTableUpgrades: {1}. StepDelta: {2}", Type, StepConst, StepDelta);

            return strBuilder.ToString();
        }
    }
}

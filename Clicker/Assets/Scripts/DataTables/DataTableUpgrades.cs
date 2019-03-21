using System.Collections.Generic;
using UnityEngine;

namespace clicker.datatables
{
    public class DataTableUpgrades
    {
        private static Dictionary<UpgradeTypes, Upgrade> m_UpgradesData;

        public static void SetData()
        {
            //TODO
            //Create LocalDataEditor
            //Get data from LocalDataEditor

            m_UpgradesData = new Dictionary<UpgradeTypes, Upgrade>();
            Dictionary<int, int> weaponSlotUpgradeValues = new Dictionary<int, int>();
            weaponSlotUpgradeValues.Add(1, 1);
            weaponSlotUpgradeValues.Add(2, 2);
            weaponSlotUpgradeValues.Add(3, 3);
            weaponSlotUpgradeValues.Add(4, 4);
            weaponSlotUpgradeValues.Add(5, 5);
            m_UpgradesData.Add(UpgradeTypes.WeaponSlot, new SlotUpgrade(weaponSlotUpgradeValues, 1f, 5, 5));
        }

        /// <summary>
        /// Получить количество шагов до следующего уровня улучшения
        /// </summary>
        /// <param name="type">Тип улучшения</param>
        /// <param name="curLvl">Текущий уровень улучшения</param>
        public static int GetStepsToNext(UpgradeTypes type, int curLvl)
        {
            if (m_UpgradesData.ContainsKey(type))
            {
                Upgrade upgrade = m_UpgradesData[type];
                if (!upgrade.IsLastLvl(curLvl))
                    return upgrade.GetStepsToNext(curLvl);
            }

            return 0;
        }

        /// <summary>
        /// Получить параметр улучшения для указанного уровня (для слотов - кол-во слотов)
        /// </summary>
        /// <param name="type">Тип улучшения</param>
        /// <param name="lvl">Уровень, </param>
        public static int GetValueForLevel(UpgradeTypes type, int lvl)
        {
            if (m_UpgradesData.ContainsKey(type))
                return m_UpgradesData[type].GetValueForLevel(lvl);

            return 1;
        }


        public enum UpgradeTypes
        {
            WeaponSlot,
            FoodSlot,
            WeaponBag,
            FoodBag
        }

        public abstract class Upgrade
        {
            protected int m_StepConst;
            protected int m_MaxValue;

            public Upgrade(int stepConst, int maxValue)
            {
                m_StepConst = stepConst;
                m_MaxValue = maxValue;
            }

            public abstract bool IsLastLvl(int lvl);

            public abstract int GetStepsToNext(int curValue);

            public abstract int GetValueForLevel(int level);
        }

        public class SlotUpgrade : Upgrade
        {
            private float m_StepDelta;
            private Dictionary<int, int> m_UpgradeValues;

            public SlotUpgrade(Dictionary<int, int> upgradeValues, float stepDelta, int stepConst, int maxValue) : base(stepConst, maxValue)
            {
                m_StepDelta = stepDelta;
                m_UpgradeValues = new Dictionary<int, int>(upgradeValues);
            }

            public override int GetStepsToNext(int curValue)
            {
                //Ex:
                //in -> curSlots = 1;
                //out <- stepsToNextValue = 5 + (curSlots - 1) * 1.4 = = 5 + 0 = 5; 

                //in -> curSlots = 2;
                //out <- stepsToNextValue = 5 + (curSlots - 1) * 1.4 = = 5 + (1 * 1.4) = 6; 

                //in -> curSlots = 2;
                //out <- stepsToNextValue = 5 + (curSlots - 1) * 1.4 = = 5 + (2 * 1.4) = 8; 

                float deltaValue = (curValue - 1) * m_StepDelta;
                int intDeltaValue = (int)deltaValue;

                return m_StepConst + intDeltaValue;
            }

            public override int GetValueForLevel(int level)
            {
                if (m_UpgradeValues.ContainsKey(level))
                    return m_UpgradeValues[level];

                return 1;
            }

            public override bool IsLastLvl(int lvl)
            {
                return lvl >= m_UpgradeValues.Count;
            }
        }

        //Add Slots:
        //- curSlotsAmount
        //- curUpgradeState
        //- stepsToNextValue


        //Add slots
        //- Weapon Slots
        //- Food slots
        //Upgrade builder
        //- Builder productivity
        //- Builder amount

        //in -> curValue
        //out <- stepsToNextValue
        //param: curSteps
    }
}
